using DevYeah.LMS.Business.ResultModels;
using Microsoft.AspNetCore.Http;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface IBlobStorage
    {
        PhotoUploadResult UploadPhoto(IFormFile photo);
    }
}
