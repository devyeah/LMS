namespace DevYeah.LMS.Business.ConfigurationModels
{
    public class TokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int Expires { get; set; }
    }
}
