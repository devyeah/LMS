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
        static MailClientMocker mailClient;
        static SignUpRequest signupRequest;
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
            mailClient = new MailClientMocker();

            signupRequest = new SignUpRequest
            {
                UserName = "devyeah",
                Email = "devyeah@gmail.com",
                Password = "123456",
                Type = (int)AccountType.Student
            };
        }

        [TestInitialize]
        public void Setup()
        {
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
        public void TestSignUpPass()
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

    }
}
