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
        protected static IOptions<TokenSettings> tokenSettings;
        protected static IOptions<ApiSettings> apiSettings;
        protected static IOptions<EmailSettings> emailSettings;

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

            var tokenConfigModel = new TokenSettings();
            configuration.GetSection("TokenSettings").Bind(tokenConfigModel);
            tokenSettings = Options.Create(tokenConfigModel);

            var apiConfigModel = new ApiSettings();
            configuration.GetSection("ApiSettings").Bind(apiConfigModel);
            apiSettings = Options.Create(apiConfigModel);

            var contactConfigModel = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(contactConfigModel);
            emailSettings = Options.Create(contactConfigModel);
        }
    }
}
