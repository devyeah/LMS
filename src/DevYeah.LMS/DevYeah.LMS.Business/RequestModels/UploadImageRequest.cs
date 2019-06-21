using Microsoft.AspNetCore.Http;

namespace DevYeah.LMS.Business.RequestModels
{
    public class UploadImageRequest
    {
        public IFormFileCollection Files { get; set; }
    }
}
