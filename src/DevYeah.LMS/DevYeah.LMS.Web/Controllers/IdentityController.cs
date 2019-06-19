using System;
using System.Collections.Generic;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace DevYeah.LMS.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public IdentityController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST api/v1/identity/signup
        [HttpPost("signup")]
        public IActionResult SignUp(SignUpRequest request)
        {
            var result = _accountService.SignUp(request);
            if (result.IsSuccess)
                return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }

        // POST api/v1/identity/signin
        [HttpPost("signin")]
        public IActionResult SignIn(SignInRequest request)
        {
            var result = _accountService.SignIn(request);
            if (result.IsSuccess)
                return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }

        // Get api/v1/identity/activate
        [HttpGet("activate")]
        public IActionResult Activate(string token)
        {
            var result = _accountService.ActivateAccount(token);
            if (result.IsSuccess)
                return Ok();

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