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
        protected static IOptions<AppSettings> appSettings;
        protected static string testRootPath;

        [ClassInitialize]
        protected static void BaseSetup(TestContext context)
        {
            #region resolve config file path
            var binDir = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            var fullDir = Assembly.GetCallingAssembly().Location;
            var indexOfPart = fullDir.IndexOf(binDir, StringComparison.OrdinalIgnoreCase);
            var basePath = fullDir.Substring(0, indexOfPart);
            testRootPath = basePath;
            #endregion

            configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettingsModel = new AppSettings();
            configuration.GetSection("LMSConfig").Bind(appSettingsModel);
            appSettings = Options.Create(appSettingsModel);
        }
    }
}
