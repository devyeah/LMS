using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevYeah.LMS.Business;
using DevYeah.LMS.Business.RequestModels;
using Microsoft.AspNetCore.Http;
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
            throw new NotImplementedException();
        }

        // POST api/v1/identity/signin
        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequest request)
        {
            throw new NotImplementedException();
        }

        // POST api/v1/identity/recoverypassword
        [HttpPost("recoverypassword")]
        public void RecoveryPassword([FromBody] string email)
        {

        }

        // POST api/v1/identity/resetpassword
        [HttpPost("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}