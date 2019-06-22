using System;
using System.Security.Claims;
using Moq;
using DevYeah.LMS.Business;
using DevYeah.LMS.Business.Helpers;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.BusinessTest.Mock;
using DevYeah.LMS.Models;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using System.IO;
using CloudinaryDotNet.Actions;

namespace DevYeah.LMS.BusinessTest
{
    [TestClass]
    public class AccountServiceTest : TestBase
    {
        MailClientMocker mailClient;
        SignUpRequest signupRequest;
        SignInRequest signInRequest;
        AccountRepositoryMocker repository;
        AccountService service;
        Account testAccount;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            BaseSetup(context);
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
            service = new AccountService(repository, mailClient, tokenSettings, apiSettings, emailTemplate, hostEnvironment, cloudinarySettings);
            testAccount = new Account
            {
                Id = Guid.Parse("bc8ee12e-cf6a-4765-a112-7c9e29469b36"),
                UserName = "devyeah",
                Email = "devyeah@gmail.com",
                Password = "f8ec997eccf015b232ac2b97992ece6caf28060d95d0cbfa6da803064e941583",
                Status = 2,
                Type = 1,
                UserProfile = new UserProfile { Id = Guid.NewGuid() }
            };
            repository.Add(testAccount);
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
            signupRequest.Email = "test@gmail.com";
            var result = service.SignUp(signupRequest);
            var signInResult = result.ResultObj as SignInResult;
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
            Assert.AreEqual("devyeah", signInResult.Username);
 
        }

        [TestMethod]
        public void TestSignUpIdenticalAccount()
        {
            var result = service.SignUp(signupRequest);
            Assert.AreEqual(IdentityResultCode.EmailConflict, result.ResultCode);
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
            var result = service.SignIn(signInRequest);
            var signInAccount = result.ResultObj as SignInResult;
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual("devyeah", signInAccount.Username);
        }

        [TestMethod]
        public void TestSignInFailed()
        {
            signInRequest.Password = "654321";
            var result = service.SignIn(signInRequest);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.PasswordError, result.ResultCode);
        }

        [TestMethod]
        public void TestInactivatedUserSignIn()
        {
            var account = repository.GetUniqueAccountByEmail("devyeah@gmail.com");
            account.Status = (int)AccountStatus.Inactive;
            repository.Update(account);
            var result = service.SignIn(signInRequest);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.InactivatedAccount, result.ResultCode);
        }

        [TestMethod]
        public void TestDeleteAccountWithEmptyArgument()
        {
            var result = service.DeleteAccount(Guid.Empty);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        [TestMethod]
        public void TestDeleteAFakeAccount()
        {
            var result = service.DeleteAccount(Guid.NewGuid());
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.AccountNotExist, result.ResultCode);
        }

        [TestMethod]
        public void TestDeleteAccountSuccess()
        {
            var result = service.DeleteAccount(testAccount.Id);
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

        [TestMethod]
        public void TestActivateAccountWithInvalidToken()
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, testAccount.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var token = IdentityHelper.GenerateToken(tokenSettings.Value.Secret, tokenSettings.Value.Issuer, tokenSettings.Value.Audience, claims, tokenSettings.Value.Expires);
            token = token.Replace('e', 'f');
            var result = service.ActivateAccount(token);
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.InvalidToken, result.ResultCode);
        }

        [TestMethod]
        public void TestActivatedAccountSuccess()
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, testAccount.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var token = IdentityHelper.GenerateToken(tokenSettings.Value.Secret, tokenSettings.Value.Issuer, tokenSettings.Value.Audience, claims, tokenSettings.Value.Expires);
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
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, testAccount.Email)
            };
            var token = IdentityHelper.GenerateToken(tokenSettings.Value.Secret, tokenSettings.Value.Issuer, tokenSettings.Value.Audience, claims, tokenSettings.Value.Expires);
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
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, testAccount.Email)
            };
            var token = IdentityHelper.GenerateToken(tokenSettings.Value.Secret, tokenSettings.Value.Issuer, tokenSettings.Value.Audience, claims, tokenSettings.Value.Expires);
            var request = new ResetPasswordRequest
            {
                Token = token,
                NewPassword = "456789"
            };
            var result = service.ResetPassword(request);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
        }

        [TestMethod]
        public void TestUploadImageFailure()
        {
            // arrangement
            ServiceResult<IdentityResultCode> result = null;
            var formFiles = new FormFileCollection();
            using (var fileStream = new FileStream(@"E:\LMS\src\DevYeah.LMS\DevYeah.LMS.BusinessTest\TestImages\hot-air-ballooning.jpg", FileMode.Open, FileAccess.Read))
            {
                var imageFile = new Mock<IFormFile>();
                imageFile.Setup(f => f.FileName).Returns("hot-air-ballooning.jpg");
                imageFile.Setup(f => f.Length).Returns(fileStream.Length);
                imageFile.Setup(f => f.OpenReadStream()).Returns(fileStream);
                formFiles.Add(imageFile.Object);

                // action
                result = service.UploadImage(new UploadImageRequest { Files = formFiles });
            }

            // assertion
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual(IdentityResultCode.UploadImageFailure, result.ResultCode);
        }
    }
}
