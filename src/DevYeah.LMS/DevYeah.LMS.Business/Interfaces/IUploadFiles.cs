using DevYeah.LMS.Business.ResultModels;
using Microsoft.AspNetCore.Http;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface IUploadFiles
    {
        PhotoUploadResult UploadPhoto(IFormFile photo);
    }
}
