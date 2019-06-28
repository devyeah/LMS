using Newtonsoft.Json.Linq;

namespace DevYeah.LMS.Business.ResultModels
{
    public class PhotoUploadResult
    {
        public bool IsSuccess { get; set; }

        public JToken JsonObj { get; set; }

        public string ErrorMessage { get; set; }
    }
}
