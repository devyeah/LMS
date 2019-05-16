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
        static AccountRepositoryMocker repository;
        static IConfiguration configuration;
        static MailClientMocker mailClient;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            repository = new AccountRepositoryMocker();

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
        }

        [TestMethod]
        public void TestSignUpArgumentIsNull()
        {
            var service = new AccountService(repository, mailClient, configuration);
            var result = service.SignUp(null);
            Assert.AreEqual(IdentityResultCode.IncompleteArgument, result.ResultCode);
        }

        [TestMethod]
        public void TestSignUpPass()
        {
            var service = new AccountService(repository, mailClient, configuration);
            var request = new SignUpRequest
            {
                UserName = "devyeah",
                Email = "devyeah@gmail.com",
                Password = "123456",
                Type = (int)AccountType.Student
            };
            var result = service.SignUp(request);
            var newAccount = result.ResultObj as Account;
            Assert.AreEqual(IdentityResultCode.Success, result.ResultCode);
            Assert.AreEqual("devyeah", newAccount.UserName);
            Assert.AreEqual("devyeah@gmail.com", newAccount.Email);
        }

        [TestMethod]
        public void TestSignUpIdenticalAccount()
        {
            var service = new AccountService(repository, mailClient, configuration);
            var request = new SignUpRequest
            {
                UserName = "devyeah",
                Email = "devyeah@gmail.com",
                Password = "123456",
                Type = (int)AccountType.Student
            };
            service.SignUp(request);
            var result = service.SignUp(request);
            Assert.AreEqual(IdentityResultCode.EmailConflict, result.ResultCode);
        }
    }
}
