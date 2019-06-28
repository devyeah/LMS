using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private static readonly string AccountNotExistMsg = "User is not exist.";
        private static readonly string PasswordErrorMsg = "Password is not correct.";
        private static readonly string InactivatedAccountMsg = "Your account has not been activated yet.";
        private static readonly string InvalidTokenMsg = "The token is invalid.";
        private static readonly string EmptyFileErrorMsg = "No file has been upload in request.";

        private readonly IAccountRepository _accountRepo;
        private readonly IEmailClient _mailClient;
        private readonly AppSettings _appSettings;
        private readonly IBlobStorage _blobStorage;

        public AccountService(IAccountRepository repository, IEmailClient mailClient,
            IOptions<AppSettings> appSettings, IBlobStorage blobStorage)
        {
            _accountRepo = repository;
            _mailClient = mailClient;
            _appSettings = appSettings.Value;
            _blobStorage = blobStorage;
        }

        public ServiceResult<IdentityResultCode> ActivateAccount(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var keyClaim = GetClaimFromToken(token, ClaimTypes.Name);
            if (keyClaim == null)
                return BuildResult(false, IdentityResultCode.InvalidToken, InvalidTokenMsg);

            try
            {
                var account = _accountRepo.GetAccount(Guid.Parse(keyClaim.Value));
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

                account.Status = (int)AccountStatus.Active;
                _accountRepo.Update(account);
                return BuildResult(true, IdentityResultCode.Success, resultObj: account);
            }
            catch (Exception ex)
            {
                return BuildResult(false, IdentityResultCode.ActivateFailure, ex.Message);
            }
        }

        public ServiceResult<IdentityResultCode> DeleteAccount(Guid accountId)
        {
            if (accountId == null || accountId.Equals(Guid.Empty))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            try
            {
                var account = _accountRepo.GetAccount(accountId);
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

                account.Status = (int)AccountStatus.Deleted;
                _accountRepo.Update(account);
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

            var account = _accountRepo.GetUniqueAccountByEmail(email);
            if (account == null)
                return;

            var subject = _appSettings.EmailTemplateConfig.PasswordRecoveryMailSubject;
            var content = BuildPasswordRecoveryMail(account);
            _mailClient.SendEmail(email, subject, content);
        }

        public ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request)
        {
            if (request == null)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var isTokenEmpty = string.IsNullOrWhiteSpace(request.Token);
            var isNewPasswordEmpty = string.IsNullOrWhiteSpace(request.NewPassword);
            if (isTokenEmpty || isNewPasswordEmpty)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var emailClaim = GetClaimFromToken(request.Token, ClaimTypes.Email);
            if (emailClaim == null)
                return BuildResult(false, IdentityResultCode.InvalidToken, InvalidTokenMsg);

            try
            {
                var account = _accountRepo.GetUniqueAccountByEmail(emailClaim.Value);
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);
                var hashedNewPassword = IdentityHelper.HashPassword(request.NewPassword);
                account.Password = hashedNewPassword;
                _accountRepo.Update(account);

                return BuildResult(true, IdentityResultCode.Success, resultObj: account);
            }
            catch (Exception ex)
            {
                return BuildResult(false, IdentityResultCode.BackendException, ex.Message);
            }
        }

        public ServiceResult<IdentityResultCode> SignIn(SignInRequest request)
        {
            var isRequestValid = ValidateSignInRequest(request);
            if (!isRequestValid)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var account = _accountRepo.GetUniqueAccountByEmail(request.Email);
            if (account == null)
                return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

            var password = IdentityHelper.HashPassword(request.Password);
            if (password != account.Password)
                return BuildResult(false, IdentityResultCode.PasswordError, PasswordErrorMsg);

            var authToken = GenerateAuthenticatedToken(account);
            var resultObject = new SignInResult { Identity = account.Id, Username = account.UserName, AuthenticatedToken = authToken };
            if ((AccountStatus)account.Status == AccountStatus.Inactive)
                return BuildResult(true, IdentityResultCode.InactivatedAccount, InactivatedAccountMsg, resultObject);

            return BuildResult(true, IdentityResultCode.Success, resultObj: resultObject);
        }

        public ServiceResult<IdentityResultCode> SignUp(SignUpRequest request)
        {
            var isRequestValid = ValidateSignUpRequest(request);
            if (!isRequestValid)
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
                UserProfile = new UserProfile { Id = Guid.NewGuid() },
            };

            try
            {
                _accountRepo.Add(newAccount);
                _accountRepo.SaveChanges();
                var subject = _appSettings.EmailTemplateConfig.SignUpMailSubject;
                var content = BuildAccountActivationMail(newAccount);
                _mailClient.SendEmail(newAccount.Email, subject, content);
                var authToken = GenerateAuthenticatedToken(newAccount);
                return BuildResult(true, IdentityResultCode.Success, SignUpSuccessMsg, new SignInResult { Identity = newAccount.Id, Username = newAccount.UserName, AuthenticatedToken = authToken });
            }
            catch (Exception ex)
            {
                return BuildResult(false, IdentityResultCode.SignUpFailure, ex.InnerException.Message);
            }
        }

        public ServiceResult<IdentityResultCode> SetAvatar(UploadImageRequest request)
        {
            var photo = request.Files.FirstOrDefault();
            if (photo == null || photo.Length == 0)
                return BuildResult(false, IdentityResultCode.SaveImageFailure, EmptyFileErrorMsg);

            var uploadResult = _blobStorage.UploadPhoto(photo);
            // Todo: update database
            if (uploadResult.IsSuccess)
                return BuildResult(true, IdentityResultCode.Success, resultObj: uploadResult.JsonObj);

            return BuildResult(false, IdentityResultCode.SaveImageFailure, uploadResult.ErrorMessage);
        }

        private bool ValidateSignInRequest(SignInRequest request)
        {
            if (request == null)
                return false;

            var isEmailEmpty = string.IsNullOrWhiteSpace(request.Email);
            var isPasswordEmpty = string.IsNullOrWhiteSpace(request.Password);
            if (isEmailEmpty || isPasswordEmpty)
                return false;

            return true;
        }

        private bool ValidateSignUpRequest(SignUpRequest request)
        {
            if (request == null)
                return false;

            var isEmailEmpty = string.IsNullOrWhiteSpace(request.Email);
            var isUserNameEmpty = string.IsNullOrWhiteSpace(request.UserName);
            var isPasswordEmpty = string.IsNullOrWhiteSpace(request.Password);
            if (isEmailEmpty || isUserNameEmpty || isPasswordEmpty)
                return false;

            return true;
        }

        private string BuildAccountActivationMail(Account account)
        {
            var token = GenerateAccountActivationToken(account);
            var link = string.Concat(_appSettings.ApiConfig.AccountActivationAPI, "?token=", token);
            var templateKey = nameof(_appSettings.EmailTemplateConfig.SignUpMailContent);
            var template = _appSettings.EmailTemplateConfig.SignUpMailContent;
            var content = RenderedEmailHelper.Parse(templateKey, template, new TemplateModel { Link = link });
            return content;
        }

        private string BuildPasswordRecoveryMail(Account account)
        {
            var token = GeneratePasswordRecoveryToken(account.Email);
            var link = string.Concat(_appSettings.ApiConfig.PasswordRecoveryAPI, "?token=", token);
            var templateKey = nameof(_appSettings.EmailTemplateConfig.PasswordRecoveryMailContent);
            var template = _appSettings.EmailTemplateConfig.PasswordRecoveryMailContent;
            var content = RenderedEmailHelper.Parse(templateKey, template, new TemplateModel { Link = link });
            return content;
        }

        private string GenerateAuthenticatedToken(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
            };

            return MakeToken(claims);
        }

        private string GenerateAccountActivationToken(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };

            return MakeToken(claims);
        }

        private string GeneratePasswordRecoveryToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            return MakeToken(claims);
        }

        private string MakeToken(Claim[] claims)
        {
            return IdentityHelper.GenerateToken(_appSettings.TokenConfig.Secret, _appSettings.TokenConfig.Issuer,
                _appSettings.TokenConfig.Audience, claims, _appSettings.TokenConfig.Expires);
        }

        private Claim GetClaimFromToken(string token, string claimType)
        {
            var principal = GetClaimsPrincipal(token);
            if (principal == null)
                return null;

            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            return identity?.FindFirst(claimType);
        }

        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.ASCII.GetBytes(_appSettings.TokenConfig.Secret);
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidIssuer = _appSettings.TokenConfig.Issuer,
                    ValidAudience = _appSettings.TokenConfig.Audience
                };

                var principal = handler.ValidateToken(token, validationParameters, out var securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
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
                var identicalAccount = _accountRepo.GetUniqueAccountByEmail(request.Email);
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
