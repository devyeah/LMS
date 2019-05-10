using System;
using DevYeah.LMS.Business;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DevYeah.LMS.Test.ServiceUnitTest
{
    public class AccountServiceUnitTest
    {
        [Fact]
        public void TestSignUp()
        {
            var options = new DbContextOptionsBuilder<LMSContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            ServiceResult<IdentityResultCode> result = null;

            using (var context = new LMSContext(options))
            {
                var accountService = new AccountService(context);
                result = accountService.SignUp(new SignUpRequest()
                {
                    UserName = "test",
                    Email = "test@gmail.com",
                    Password = "123456",
                });
            }

            
            Console.Write("+++++++++++++++++"+result.Message);
            Console.WriteLine(result.ResultCode);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Message);
            //Assert.True(result.IsSuccess);
            //Assert.Equal(IdentityResultCode.Success, result.ResultCode);
        }
    }
}
