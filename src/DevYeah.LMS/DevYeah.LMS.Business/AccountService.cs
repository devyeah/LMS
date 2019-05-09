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
            var account = new Account()
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                UserProfile = new UserProfile(),
            };

            _dbContext.Account.Add(account);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                return new ServiceResult<IdentityResultCode>()
                {
                    IsSuccess = false,
                    ResultCode = IdentityResultCode.AccountSaveFailure,
                    Message = ex.Message,
                    ResultObj = null,
                };
            }

            return new ServiceResult<IdentityResultCode>()
            {
                IsSuccess = true,
                ResultCode = IdentityResultCode.Success,
                Message = "",
                ResultObj = account,
            };
        }
    }
}
