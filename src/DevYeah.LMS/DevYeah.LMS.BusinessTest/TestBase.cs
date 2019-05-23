using System;
using System.IO;
using System.Reflection;
using DevYeah.LMS.Business.ConfigurationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevYeah.LMS.BusinessTest
{
    public class TestBase
    {
        protected static IConfiguration configuration;
        protected static IOptions<TokenManagement> tokenManagement;
        protected static IOptions<ApiManagement> apiManagement;
        protected static IOptions<ContactManagement> contactManagement;

        [ClassInitialize]
        protected static void BaseSetup(TestContext context)
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
    }
}
