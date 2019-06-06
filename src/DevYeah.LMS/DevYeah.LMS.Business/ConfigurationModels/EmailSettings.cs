namespace DevYeah.LMS.Business.ConfigurationModels
{
    public class EmailSettings
    {
        public string OfficialEmailAddress { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
    }
}
