using System;
using System.IO;
using System.Linq;
using System.Threading;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevYeah.LMS.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevYeah.LMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly static string EmptyFileErrorMsg = "No file has been upload in request.";

        private readonly static string ImageUploadFailMsg = "Uploading of image is failed";

        private readonly AccountService _accountService;

        private readonly Cloudinary _cloudinary;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
            _cloudinary = new Cloudinary();
        }

        // POST api/v1/identity/uploadphoto
        [HttpPost("uploadphoto")]
        public IActionResult UploadImage()
        {
            var image = Request.Form.Files.FirstOrDefault();
            if (image == null || image.Length == 0)
                return BadRequest(EmptyFileErrorMsg);

            var localPath = SaveImage(image, out string imageName);
            if (imageName == null)
                return BadRequest(ImageUploadFailMsg);

            var uploadResult = RetryUpload(() => UploadImage(localPath, imageName), 3);
            if (uploadResult == null)
                return BadRequest(ImageUploadFailMsg);

            return Ok(uploadResult.JsonObj);
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
                    if (result != null)
                        return result;
                }
                catch (Exception)
                {
                    logImportant?.Invoke();
                    Thread.Sleep(1000);
                }
            } while (loopCounter < maxRetryCounter);
            if (loopCounter > 1)
                logError?.Invoke();
            return null;
        }

        private ImageUploadResult UploadImage(string path, string name)
        {
            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(path),
                PublicId = name
            };
            return _cloudinary.Upload(uploadParam);
        }

        private string SaveImage(IFormFile image, out string imageName)
        {
            var suffix = Path.GetExtension(image.FileName);
            imageName = $"{Guid.NewGuid().ToString()}{suffix}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageName);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                return filePath;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}