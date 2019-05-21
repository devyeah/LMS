using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.Extensions.Configuration;
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
        private readonly IMailClient _mailClient;
        private readonly TokenManagement _tokenManagement;
        private readonly ApiManagement _apiManagement;
        private readonly ContactManagement _contactManagement;

        public AccountService(IAccountRepository repository, IMailClient mailClient, 
            IOptions<TokenManagement> tokenManagement, IOptions<ApiManagement> apiManagement, 
            IOptions<ContactManagement> contactManagement)
        {
            _repository = repository;
            _mailClient = mailClient;
            _tokenManagement = tokenManagement.Value;
            _apiManagement = apiManagement.Value;
            _contactManagement = contactManagement.Value;
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

                account.Status = (int)AccountStatus.Activated;
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

            ClaimsIdentity identity = null;
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
                var secretKey = Encoding.ASCII.GetBytes(_tokenManagement.Secret);
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidIssuer = _tokenManagement.Issuer,
                    ValidAudience = _tokenManagement.Audience
                };

                var principal = handler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public ServiceResult<IdentityResultCode> InvalidAccount(Guid accountId)
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
                return BuildResult(true, IdentityResultCode.Success, resultObj:account);
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

            SendEmail(BuildPasswordRecoveryMail(account));
        }

        private string GeneratePasswordRecoveryToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, email)
            };
            var token = GenerateToken(claims);
            return token;
        }

        public ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request)
        {
            if (request == null || 
                string.IsNullOrWhiteSpace(request.Token) || 
                string.IsNullOrWhiteSpace(request.NewPassword))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            Claim emailClaim = GetClaimFromToken(request.Token, ClaimTypes.Email);
            if (emailClaim == null)
                return BuildResult(false, IdentityResultCode.InvalidToken, InvalidTokenMsg);

            try
            {
                var account = _repository.GetUniqueAccountByEmail(emailClaim.Value);
                if (account == null)
                    return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);
                var hashedNewPassword = HashPassword(request.NewPassword);
                account.Password = hashedNewPassword;
                _repository.Update(account);

                return BuildResult(true, IdentityResultCode.Success, resultObj:account);
            }
            catch (Exception ex)
            {
                return BuildResult(false, IdentityResultCode.BackendException, ex.Message);
            }
        }

        public ServiceResult<IdentityResultCode> SignIn(SignInRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var account = _repository.GetUniqueAccountByEmail(request.Email);
            if (account == null)
                return BuildResult(false, IdentityResultCode.AccountNotExist, AccountNotExistMsg);

            var password = HashPassword(request.Password);
            if (!password.Equals(account.Password))
                return BuildResult(false, IdentityResultCode.PasswordError, PasswordErrorMsg);

            if (account.Status == (int)AccountStatus.Inactivated)
                return BuildResult(true, IdentityResultCode.InactivatedAccount, InactivatedAccountMsg, account);

            return BuildResult(true, IdentityResultCode.Success, resultObj:account);
        }

        public ServiceResult<IdentityResultCode> SignUp(SignUpRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var isEmailExist = CheckDuplicateEmailAddress(request);
            if (isEmailExist)
                return BuildResult(false, IdentityResultCode.EmailConflict, EmailConflictMsg);

            string hashedPassword = HashPassword(request.Password);
            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                Password = hashedPassword,
                Status = (int)AccountStatus.Inactivated,
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
                var isMailSent = SendEmail(BuildAccountActivationMail(newAccount));
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

        private MailMessage BuildAccountActivationMail(Account account)
        {
            var token = GenerateAccountActivationToken(account);
            var mailMessage = new MailMessage
            (
                from: _contactManagement.OfficalEmailAddress,
                to: account.Email,
                subject: SubjectOfActivateEmail,
                body: string.Concat(_apiManagement.AccountActivationAPI, "?token=", token)
            );
            return mailMessage;
        }

        private MailMessage BuildPasswordRecoveryMail(Account account)
        {
            var token = GeneratePasswordRecoveryToken(account.Email);
            var mailMessage = new MailMessage
            (
                from: _contactManagement.OfficalEmailAddress,
                to: account.Email,
                subject: PasswordRecoveryEmail,
                body: string.Concat(_apiManagement.PasswordRecoveryAPI, "?token=", token)
            );
            return mailMessage;
        }

        private bool SendEmail(MailMessage mailMessage)
        {
            var loopCounter = 0;
            // If sending email fail then trying 2 more times
            do
            {
                _mailClient.Send(mailMessage);
                loopCounter++;
                if (loopCounter >= 3)
                    break;
            } while (!_mailClient.MailSent);

            return _mailClient.MailSent;
        }

        private string GenerateAccountActivationToken(Account account)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var token = GenerateToken(claims);
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

        private static string HashPassword(string password)
        {
            string md5Password = null;
            string sha256Password = null;
            byte[] data;
            using (var md5Hash = MD5.Create())
            {
                data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                md5Password = BuildHexadecimalString(data);
            }

            using (var sha256 = SHA256.Create())
            {
                data = sha256.ComputeHash(Encoding.UTF8.GetBytes(md5Password));
                sha256Password =  BuildHexadecimalString(data);
            }
            
            return sha256Password;
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

        private string GenerateToken(Claim[] claims)
        {
            var secretKey = Encoding.ASCII.GetBytes(_tokenManagement.Secret);
            
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _tokenManagement.Issuer,
                Audience = _tokenManagement.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var emailToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(emailToken);
        }

        private static string BuildHexadecimalString(byte[] data)
        {
            var strBuilder = new StringBuilder();

            foreach (var character in data)
            {
                strBuilder.Append(character.ToString("x2"));

            }

            return strBuilder.ToString();
        }
    }
}
