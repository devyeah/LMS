using System;
using System.IO;
using System.Reflection;
using DevYeah.LMS.Business;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.BusinessTest.Mock;
using DevYeah.LMS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevYeah.LMS.BusinessTest
{
    [TestClass]
    public class AccountServiceTest
    {
        static IConfiguration configuration;
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
            service = new AccountService(repository, mailClient, configuration);
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
            service.SignUp(signupRequest);
            var account = repository.GetUniqueAccountByEmail(signupRequest.Email);
            account.Status = (int)AccountStatus.Activated;
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
            service.SignUp(signupRequest);
            var account = repository.GetUniqueAccountByEmail(signupRequest.Email);
            account.Status = (int)AccountStatus.Activated;
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
    }
}