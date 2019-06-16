using System;
using DevYeah.LMS.Business;
using DevYeah.LMS.Business.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace DevYeah.LMS.Web.Controllers
{
    [Route("api/v1/[identity]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly AccountService _accountService;

        public IdentityController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // POST api/v1/identity/signup
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            var result = _accountService.SignUp(request);
            if (result.IsSuccess)
                return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }

        // POST api/v1/identity/signin
        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequest request)
        {
            var result = _accountService.SignIn(request);
            if (result.IsSuccess)
                return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }

        // POST api/v1/identity/recoverypassword
        [HttpPost("recoverypassword")]
        public void RecoveryPassword([FromBody] string email)
        {
            _accountService.RecoverPassword(email);
        }

        // POST api/v1/identity/resetpassword
        [HttpPost("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = _accountService.ResetPassword(request);
            if (result.IsSuccess)
                return Ok();

            return BadRequest(result.Message);
        }
    }
}