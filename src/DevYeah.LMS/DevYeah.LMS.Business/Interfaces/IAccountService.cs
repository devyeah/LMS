using System;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface IAccountService
    {
        ServiceResult<IdentityResultCode> SignUp(SignUpRequest request);
        ServiceResult<IdentityResultCode> SignIn(SignInRequest request);
        void RecoverPassword(string email);
        ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request);
        ServiceResult<IdentityResultCode> DeleteAccount(Guid accountId);
        ServiceResult<IdentityResultCode> ActivateAccount(string token);
        ServiceResult<IdentityResultCode> SetAvatar(UploadImageRequest request);
    }
}
