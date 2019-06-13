using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevYeah.LMS.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevYeah.LMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // POST api/v1/identity/uploadphoto
        [HttpPost("uploadphoto")]
        public IActionResult UploadPhoto()
        {
            throw new NotImplementedException();
        }

        // POST api/v1/identity/
    }
}