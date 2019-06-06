using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Helpers;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevYeah.LMS.Business
{
    public class AccountService : IAccountService
    {
        private static readonly string ArgumentNullMsg = "The necessary information is incomplete.";
        private static readonly string EmailConflictMsg = "This email has been used.";
        private static readonly string SignUpSuccessMsg = "You has signed up successfully, please active your account through the email we sent to you.";
        private static readonly string ActivateMailSendFailMsg = "your sign up failed. Please try again later.";
        private static readonly string AccountNotExistMsg = "User is not exist.";
        private static readonly string PasswordErrorMsg = "Password is not correct.";
        private static readonly string InactivatedAccountMsg = "Your account has not been activated yet.";
        private static readonly string SubjectOfActivateEmail = "Thank you for signing up, Please click the link below to activate your account.";
        private static readonly string PasswordRecoveryEmail = "Please click the link below to reset your password.";
        private static readonly string InvalidTokenMsg = "The token is invalid.";
        private static readonly string ActivationFailMsg = "Your account was not able to be activated, please try again later.";

        private readonly IAccountRepository _repository;
        private readonly IEmailClient _mailClient;
        private readonly TokenSettings _tokenSettings;
        private readonly ApiSettings _apiSettings;
        private readonly EmailSettings _emailSettings;
        private readonly EmailTemplate _emailTemplate;

        public AccountService(IAccountRepository repository, IEmailClient mailClient,
            IOptions<TokenSettings> tokenSettings, IOptions<ApiSettings> apiSettings,
            IOptions<EmailSettings> emailSettings, IOptions<EmailTemplate> emailTemplate)
        {
            _repository = repository;
            _mailClient = mailClient;
            _tokenSettings = tokenSettings.Value;
            _apiSettings = apiSettings.Value;
            _emailSettings = emailSettings.Value;
            _emailTemplate = emailTemplate.Value;
        }

        public ServiceResult<IdentityResultCode> ActivateAccount(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            Claim keyClaim = GetClaimFromToken(token, ClaimTypes.Name);
            if (keyClaim == null)
                return BuildResult(false, IdentityResultCode.InvalidToken, InvalidTokenMsg);

            try
            {
                var account = _repository.GetAccount(Guid.Parse(keyClaim.Value));
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

                account.Status = (int)AccountStatus.Active;
                _repository.Update(account);
                return BuildResult(true, IdentityResultCode.Success, resultObj: account);
            }
            catch (Exception ex)
            {

                return BuildResult(false, IdentityResultCode.ActivateFailure, ex.Message);
            }
        }

        private Claim GetClaimFromToken(string token, string claimType)
        {
            var principal = GetClaimsPrincipal(token);
            if (principal == null)
                return null;

            ClaimsIdentity identity;
            try
            {
                identity = principal.Identity as ClaimsIdentity;
            }
            catch (NullReferenceException)
            {

                return null;
            }

            return identity.FindFirst(claimType);
        }

        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidAudience = _tokenSettings.Audience
                };

                var principal = handler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public ServiceResult<IdentityResultCode> DeleteAccount(Guid accountId)
        {
            if (accountId == null || accountId.Equals(Guid.Empty))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            try
            {
                var account = _repository.GetAccount(accountId);
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

                account.Status = (int)AccountStatus.Deleted;
                _repository.Update(account);
                return BuildResult(true, IdentityResultCode.Success, resultObj: account);
            }
            catch (Exception ex)
            {

                return BuildResult(false, IdentityResultCode.BackendException, ex.Message);
            }
        }

        public void RecoverPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            var account = _repository.GetUniqueAccountByEmail(email);
            if (account == null)
                return;

            var message = BuildPasswordRecoveryMail(account);
            var subject = _emailTemplate.PasswordRecoveryMailSubject;
            SendEmail(email, subject, message);
        }

        private string GeneratePasswordRecoveryToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, email)
            };
            var token = IdentityHelper
                .GenerateToken(_tokenSettings.Secret, _tokenSettings.Issuer,
                _tokenSettings.Audience, claims, _tokenSettings.Expires);
            return token;
        }

        public ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request)
        {
            var isRequestNull = (request == null);
            if (isRequestNull)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var isTokenEmpty = string.IsNullOrWhiteSpace(request.Token);
            var isNewPasswordEmpty = string.IsNullOrWhiteSpace(request.NewPassword);

            if (isTokenEmpty || isNewPasswordEmpty)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            Claim emailClaim = GetClaimFromToken(request.Token, ClaimTypes.Email);
            if (emailClaim == null)
                return BuildResult(false, IdentityResultCode.InvalidToken, InvalidTokenMsg);

            try
            {
                var account = _repository.GetUniqueAccountByEmail(emailClaim.Value);
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);
                var hashedNewPassword = IdentityHelper.HashPassword(request.NewPassword);
                account.Password = hashedNewPassword;
                _repository.Update(account);

                return BuildResult(true, IdentityResultCode.Success, resultObj: account);
            }
            catch (Exception ex)
            {
                return BuildResult(false, IdentityResultCode.BackendException, ex.Message);
            }
        }

        public ServiceResult<IdentityResultCode> SignIn(SignInRequest request)
        {
            var isRequestNull = request == null;
            if (isRequestNull)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var isEmailEmpty = string.IsNullOrWhiteSpace(request.Email);
            var isPasswordEmpty = string.IsNullOrWhiteSpace(request.Password);

            if (isEmailEmpty || isPasswordEmpty)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var account = _repository.GetUniqueAccountByEmail(request.Email);
            if (account == null)
                return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

            var password = IdentityHelper.HashPassword(request.Password);
            if (password != account.Password)
                return BuildResult(false, IdentityResultCode.PasswordError, PasswordErrorMsg);

            if ((AccountStatus)account.Status == AccountStatus.Inactive)
                return BuildResult(true, IdentityResultCode.InactivatedAccount, InactivatedAccountMsg, account);

            return BuildResult(true, IdentityResultCode.Success, resultObj: account);
        }

        public ServiceResult<IdentityResultCode> SignUp(SignUpRequest request)
        {
            var isRequestNull = request == null;
            if (isRequestNull)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var isEmailEmpty = string.IsNullOrWhiteSpace(request.Email);
            var isUserNameEmpty = string.IsNullOrWhiteSpace(request.UserName);
            var isPasswordEmpty = string.IsNullOrWhiteSpace(request.Password);

            if (isEmailEmpty || isUserNameEmpty || isPasswordEmpty)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var isEmailExist = CheckDuplicateEmailAddress(request);
            if (isEmailExist)
                return BuildResult(false, IdentityResultCode.EmailConflict, EmailConflictMsg);

            string hashedPassword = IdentityHelper.HashPassword(request.Password);
            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                Password = hashedPassword,
                Status = (int)AccountStatus.Inactive,
                Type = request.Type,
                UserProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                },
            };

            try
            {
                _repository.Add(newAccount);
                _repository.SaveChanges();
                var isMailSent = SendEmail(newAccount.Email, _emailTemplate.SignUpMailSubject, BuildAccountActivationMail(newAccount));
                if (!isMailSent)
                {
                    _repository.Delete(newAccount);
                    _repository.SaveChanges();
                    return BuildResult(false, IdentityResultCode.EmailError, ActivateMailSendFailMsg);
                }
                return BuildResult(true, IdentityResultCode.Success, SignUpSuccessMsg, newAccount);
            }
            catch (Exception ex)
            {
                return BuildResult(false, IdentityResultCode.SignUpFailure, ex.InnerException.Message);
            }
        }

        private string BuildAccountActivationMail(Account account)
        {
            var token = GenerateAccountActivationToken(account);
            var link = string.Concat(_apiSettings.AccountActivationAPI, "?token=", token);
            var templateKey = nameof(_emailTemplate.SignUpMailContent);
            var template = _emailTemplate.SignUpMailContent;
            var content = RenderedEmailHelper.Parse(templateKey, template, new TemplateModel { Link = link });
            return content;
        }

        private string BuildPasswordRecoveryMail(Account account)
        {
            var token = GeneratePasswordRecoveryToken(account.Email);
            var link = string.Concat(_apiSettings.PasswordRecoveryAPI, "?token=", token);
            var templateKey = nameof(_emailTemplate.PasswordRecoveryMailContent);
            var template = _emailTemplate.PasswordRecoveryMailContent;
            var content = RenderedEmailHelper.Parse(templateKey, template, new TemplateModel { Link = link });
            return content;
        }

        private bool SendEmail(string emailAddress, string subject, string content)
        {
            var loopCounter = 0;
            var isSuccess = false;
            // If sending email fail then trying another 2 times
            do
            {
                loopCounter++;
                try
                {
                    _mailClient.SendEmail(emailAddress, subject, content);
                }
                catch (Exception)
                {
                    if (loopCounter < 3)
                        continue;
                    else
                        break;
                }
                isSuccess = true;
                if (isSuccess == true)
                    break;
            } while (loopCounter < 3);
            
            return isSuccess;
        }

        private string GenerateAccountActivationToken(Account account)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var token = IdentityHelper
                .GenerateToken(_tokenSettings.Secret, _tokenSettings.Issuer,
                _tokenSettings.Audience, claims, _tokenSettings.Expires);
            return token;
        }

        private static ServiceResult<IdentityResultCode> BuildResult(bool isSuccess, IdentityResultCode code, string message = "", object resultObj = null)
        {
            return new ServiceResult<IdentityResultCode>
            {
                IsSuccess = isSuccess,
                ResultCode = code,
                Message = message,
                ResultObj = resultObj
            };
        }

        private bool CheckDuplicateEmailAddress(SignUpRequest request)
        {
            try
            {
                var identicalAccount = _repository.GetUniqueAccountByEmail(request.Email);
                if (identicalAccount != null)
                    return true;

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }


    }
}
