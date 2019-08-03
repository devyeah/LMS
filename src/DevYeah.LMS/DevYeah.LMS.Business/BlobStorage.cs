using System;
using System.IO;
using System.Net;
using System.Threading;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.ResultModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DevYeah.LMS.Business
{
    public class BlobStorage : IBlobStorage
    {
        private static readonly string INTERNAL_ERROR_MSG = "File Uploading Is Failed.";

        private readonly AppSettings _appSettings;

        private readonly Cloudinary _cloudinaryStorage;

        public BlobStorage(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            var cloudinaryAccount = new Account(_appSettings.CloudinaryConfig.CloudName, _appSettings.CloudinaryConfig.APIKey, _appSettings.CloudinaryConfig.APISecret);
            _cloudinaryStorage = new Cloudinary(cloudinaryAccount);
        }
        public PhotoUploadResult UploadPhoto(IFormFile photo)
        {
            var uploadParams = MakeUploadParams(photo);
            if (uploadParams == null)
                return new PhotoUploadResult { IsSuccess = false, JsonObj = null, ErrorMessage = INTERNAL_ERROR_MSG };

            var uploadResult = RetryUpload(() => _cloudinaryStorage.Upload(uploadParams), _appSettings.MaxRetryCount);
            if (uploadResult.StatusCode == HttpStatusCode.OK)
                return new PhotoUploadResult { IsSuccess = true, JsonObj = uploadResult.JsonObj, ErrorMessage = null };

            return new PhotoUploadResult { IsSuccess = false, JsonObj = null, ErrorMessage = uploadResult.Error.Message };
        }

        private ImageUploadParams MakeUploadParams(IFormFile image)
        {
            var imageName = GetUniqueName(image);
            if (string.IsNullOrWhiteSpace(imageName)) return null;

            var readStream = image.OpenReadStream();
            if (readStream == null) return null;

            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(imageName, readStream),
                PublicId = $"{_appSettings.CloudinaryConfig.AvatarFolder}/{imageName}"
            };
            return uploadParam;
        }

        private string GetUniqueName(IFormFile image)
        {
            try
            {
                var suffix = Path.GetExtension(image.FileName);
                var imageName = $"{Guid.NewGuid().ToString()}{suffix}";
                return imageName;
            }
            catch (Exception)
            {
                // Log exceptions
                return null;
            }
        }

        private ImageUploadResult RetryUpload(Func<ImageUploadResult> logic, int maxRetryCounter, Action logImportant = null, Action logError = null)
        {
            var loopCounter = 0;
            // If uploading image fail then trying another 2 times
            do
            {
                loopCounter++;
                try
                {
                    var result = logic?.Invoke();
                    if (string.IsNullOrEmpty(result.Error.Message))
                        return result;
                }
                catch (Exception)
                {
                    logImportant?.Invoke();
                    Thread.Sleep(_appSettings.SleepPeriod);
                }
            } while (loopCounter < maxRetryCounter);
            if (loopCounter > 1)
                logError?.Invoke();
            return null;
        }
    }
}
