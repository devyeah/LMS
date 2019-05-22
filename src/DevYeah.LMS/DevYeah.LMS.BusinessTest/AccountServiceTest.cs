using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using DevYeah.LMS.Business;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.BusinessTest.Mock;
using DevYeah.LMS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevYeah.LMS.BusinessTest
{
    [TestClass]
    public class AccountServiceTest
    {
        static IConfiguration configuration;
        static IOptions<TokenManagement> tokenManagement;
        static IOptions<ApiManagement> apiManagement;
        static IOptions<ContactManagement> contactManagement;
        MailClientMocker mailClient;
        SignUpRequest signupRequest;
        SignInRequest signInRequest;
        AccountRepositoryMocker repository;
        AccountService service;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            #region resolve config file path
            var binDir = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            var fullDir = Assembly.GetCallingAssembly().Location;
            var indexOfPart = fullDir.IndexOf(binDir, StringComparison.OrdinalIgnoreCase);
            var basePath = fullDir.Substring(0, indexOfPart);
            #endregion

            configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var tokenConfigModel = new TokenManagement();
            configuration.GetSection("TokenManagement").Bind(tokenConfigModel);
            tokenManagement = Options.Create(tokenConfigModel);

            var apiConfigModel = new ApiManagement();
            configuration.GetSection("ApiManagement").Bind(apiConfigModel);
            apiManagement = Options.Create(apiConfigModel);

            var contactConfigModel = new ContactManagement();
            configuration.GetSection("ContactManagement").Bind(contactConfigModel);
            contactManagement = Options.Create(contactConfigModel);
        }

        [TestInitialize]
        public void Setup()
        {
            mailClient = new MailClientMocker();

            signupRequest = new SignUpRequest
            {
                UserName = "devyeah",
                Email = "devyeah@gmail.com",
                Password = "123456",
                Type = (int)AccountType.Student
            };
            signInRequest = new SignInRequest
            {
                Email = "devyeah@gmail.com",
                Password = "123456"
            };
            repository = new AccountRepositoryMocker();
            service = new AccountService(repository, mailClient, tokenManagement, apiManagement, contactManagement);
        }

        [TestMethod]
        public void TestSignUpArgumentIsNull()
        {
            var result = service.SignUp(null);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        [TestMethod]
        public void TestSignUpSuccess()
        {
            var result = service.SignUp(signupRequest);
            var newAccount = result.ResultObj as Account;
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
            Assert.AreEqual("devyeah", newAccount.UserName);
            Assert.AreEqual("devyeah@gmail.com", newAccount.Email);
        }

        [TestMethod]
        public void TestSignUpIdenticalAccount()
        {
            service.SignUp(signupRequest);
            var result = service.SignUp(signupRequest);
            Assert.AreEqual(IdentityResultCode.EmailConflict, result.ResultCode);
        }

        [TestMethod]
        public void TestSignUpFailCausedByActivatedMail()
        {
            mailClient.MailSent = false;
            var result = service.SignUp(signupRequest);
            Assert.AreEqual(IdentityResultCode.EmailError, result.ResultCode);
        }

        [TestMethod]
        public void TestSignInWithNullArguments()
        {
            var result = service.SignIn(null);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        [TestMethod]
        public void TestSignInSuccess()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            account.Status = (int)AccountStatus.Active;
            repository.Update(account);
            var result = service.SignIn(signInRequest);
            var signInAccount = result.ResultObj as Account;
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual("devyeah@gmail.com", signInAccount.Email);
        }

        [TestMethod]
        public void TestSignInFailed()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            account.Status = (int)AccountStatus.Active;
            repository.Update(account);
            signInRequest.Password = "654321";
            var result = service.SignIn(signInRequest);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.PasswordError, result.ResultCode);
        }

        [TestMethod]
        public void TestInactivatedUserSignIn()
        {
            service.SignUp(signupRequest);
            var result = service.SignIn(signInRequest);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.InactivatedAccount, result.ResultCode);
        }

        [TestMethod]
        public void TestInvalidAccountWithEmptyArgument()
        {
            var result = service.DeleteAccount(Guid.Empty);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        [TestMethod]
        public void TestInvalidAFakeAccount()
        {
            var result = service.DeleteAccount(Guid.NewGuid());
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.AccountNotExist, result.ResultCode);
        }

        [TestMethod]
        public void TestInvalidAccountSuccess()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            var result = service.DeleteAccount(account.Id);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
        }

        [TestMethod]
        public void TestActivateAccountWithNullArgument()
        {
            var result = service.ActivateAccount(null);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        private string GenerateToken(Claim[] claims)
        {
            var secretKey = Encoding.ASCII.GetBytes(tokenManagement.Value.Secret);
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = tokenManagement.Value.Issuer,
                Audience = tokenManagement.Value.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var emailToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(emailToken);
        }

        [TestMethod]
        public void TestActivateAccountWithInvalidToken()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var token = GenerateToken(claims);
            token = token.Replace('e', 'f');
            var result = service.ActivateAccount(token);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.InvalidToken, result.ResultCode);
        }

        [TestMethod]
        public void TestActivatedAccountSuccess()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var token = GenerateToken(claims);
            var result = service.ActivateAccount(token);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
        }

        [TestMethod]
        public void TestResetPasswordWithNullArgument()
        {
            var result = service.ResetPassword(null);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        [TestMethod]
        public void TestResetPasswordWithInvalidToken()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, account.Email)
            };
            var token = GenerateToken(claims);
            token = token.Replace('e', 'f');
            var request = new ResetPasswordRequest
            {
                Token = token,
                NewPassword = "456789"
            };
            var result = service.ResetPassword(request);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.InvalidToken, result.ResultCode);
        }

        [TestMethod]
        public void TestResetPasswordSuccess()
        {
            var signUpResult = service.SignUp(signupRequest);
            var account = signUpResult.ResultObj as Account;
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, account.Email)
            };
            var token = GenerateToken(claims);
            var request = new ResetPasswordRequest
            {
                Token = token,
                NewPassword = "456789"
            };
            var result = service.ResetPassword(request);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
        }
    }
}
