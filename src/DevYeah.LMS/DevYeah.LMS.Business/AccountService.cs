using System;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Models;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Business
{
    public class AccountService : IAccountService
    {
        private readonly LMSContext _dbContext;
        public AccountService(LMSContext context)
        {
            this._dbContext = context;
        }
        public ServiceResult<IdentityResultCode> InvalidAccount(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void RecoverPassword(string email)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<IdentityResultCode> SignIn(SignInRequest request)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<IdentityResultCode> SignUp(SignUpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
