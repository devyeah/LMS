using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DevYeah.LMS.Web.Helpers
{
    public static class ImageUploadHelper
    {
        public static string GetImageSuffix(IFormFile image)
        {
            var imageName = image.FileName;
            var suffix = "gif";
            var isEmptyName = string.IsNullOrWhiteSpace(imageName);
            var isNotSuffix = imageName.IndexOf('.') == -1;
            if (isEmptyName || isNotSuffix)
                return suffix;
            
            var speratedNames = imageName.Split(".");
            suffix= speratedNames[speratedNames.Length - 1];
            return suffix;
        }
    }
}
