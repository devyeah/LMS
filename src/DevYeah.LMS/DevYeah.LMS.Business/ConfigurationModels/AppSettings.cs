namespace DevYeah.LMS.Business.ConfigurationModels
{
    public class AppSettings
    {
        public int MaxRetryCount { get; set; }

        public int SleepPeriod { get; set; }

        public string DefaultCourseScreenCast { get; set; }

        public DbSettings DbConfig { get; set; } = new DbSettings();

        public ApiSettings ApiConfig { get; set; } = new ApiSettings();

        public CloudinarySettings CloudinaryConfig { get; set; } = new CloudinarySettings();

        public EmailSettings EmailConfig { get; set; } = new EmailSettings();

        public EmailTemplateSettings EmailTemplateConfig { get; set; } = new EmailTemplateSettings();

        public TokenSettings TokenConfig { get; set; } = new TokenSettings();
    }
}
