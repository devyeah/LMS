using System;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace DevYeah.LMS.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public IdentityController(IAccountService accountService) => _accountService = accountService;

        [HttpPost("register")]
        public IActionResult SignUp(SignUpRequest request) => GetResult(() => _accountService.SignUp(request));

        [HttpPost("login")]
        public IActionResult SignIn(SignInRequest request) => GetResult(() => _accountService.SignIn(request));

        [HttpPost("launch")]
        public IActionResult Activate(string token) => GetResult(() => _accountService.ActivateAccount(token));

        [HttpPost("recoverypassword")]
        public void SendRecoverEmail(string email) => _accountService.RecoverPassword(email);

        [HttpPost("updatepassword")]
        public IActionResult ResetPassword(ResetPasswordRequest request) => GetResult(() => _accountService.ResetPassword(request));

        [HttpGet]
        public IActionResult FetchAvatar()
        {
            return null;
        }

        [HttpPost("uploadphoto")]
        public IActionResult UploadImage(UploadImageRequest request) => GetResult(() => _accountService.SetAvatar(request));

        private IActionResult GetResult(Func<ServiceResult<IdentityResultCode>> action)
        {
            var result = action();
            if (result.IsSuccess)
                return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }
    }
}