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

        // POST api/v1/identity/signup
        [HttpPost("register")]
        public IActionResult SignUp(SignUpRequest request) => GetResult(() =>_accountService.SignUp(request));

        // POST api/v1/identity/signin
        [HttpPost("login")]
        public IActionResult SignIn(SignInRequest request) => GetResult(() => _accountService.SignIn(request));

        // Get api/v1/identity/activate
        [HttpGet("active")]
        public IActionResult Activate(string token) => GetResult(() => _accountService.ActivateAccount(token));

        // POST api/v1/identity/recoverypassword
        [HttpPost("recoverypassword")]
        public void SendRecoverEmail(string email) => _accountService.RecoverPassword(email);

        // POST api/v1/identity/resetpassword
        [HttpPost("updatepassword")]
        public IActionResult ResetPassword(ResetPasswordRequest request) => GetResult(() => _accountService.ResetPassword(request));

        private IActionResult GetResult(Func<ServiceResult<IdentityResultCode>> action)
        {
            var result = action();
            if (result.IsSuccess)
                return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }
    }
}