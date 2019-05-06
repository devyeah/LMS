using DevYeah.LMS.Models;
using System;


namespace DevYeah.LMS.Business.Interfaces
{
    interface IAccountService
    {
        // return an Id of the account that has been created
        Guid? SignUp(Account account);
        Account SignIn(string email, String password);
        void RecoverPassword(string email);
        bool ResetPassword(Guid accountId, string newPassword);
        void DeleteAccountByKey(Guid accountId);
    }
}
