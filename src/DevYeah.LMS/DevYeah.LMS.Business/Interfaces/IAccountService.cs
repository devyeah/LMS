using System;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    interface IAccountService
    {
        // return an Id of the account that has been created
        ServiceResult<IdentityResultCode> SignUp(SignUpRequest request);
        ServiceResult<IdentityResultCode> SignIn(SignInRequest request);
        void RecoverPassword(string email);
        ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request);
        ServiceResult<IdentityResultCode> InvalidAccount(Guid accountId);
        ServiceResult<IdentityResultCode> ActivateAccount(string token);
    }
}
