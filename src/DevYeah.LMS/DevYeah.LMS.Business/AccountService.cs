using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DevYeah.LMS.Business
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMailClient _mailClient;
        private readonly IConfiguration _configuration;

        private static readonly string ArgumentNullMsg = "The necessary information is incomplete.";
        private static readonly string EmailConflictMsg = "This email has been used.";
        private static readonly string SignUpSuccessMsg = "You has signed up successfully, please active your account through the email we sent to you.";
        private static readonly string ActivateMailSendFailMsg = "Because some unknown reason, your sign up failed. Please try again later.";
        private static readonly string AccountNotExistMsg = "User is not exist.";
        private static readonly string PasswordErrorMsg = "Password is not correct.";
        private static readonly string InactivatedAccountMsg = "Your account has not been activated yet.";
        private static readonly string SubjectOfActivateEmail = "Thank you for signing up, Please click the link below to activate your account.";

        public AccountService(IAccountRepository repository, IMailClient mailClient, IConfiguration configuration)
        {
            _repository = repository;
            _mailClient = mailClient;
            _configuration = configuration;
        }
        public ServiceResult<IdentityResultCode> ActivateAccount(string token)
        {
            //var handler = new JwtSecurityTokenHandler();
            ////var emailToken = handler.ReadJwtToken(token);
            //var secretkey = Encoding.ASCII.GetBytes(_configuration["secret"]);
            //var param = new TokenValidationParameters
            //{
            //    ClockSkew = TimeSpan.FromMinutes(1),
            //    ValidIssuer = "DevYeah",
            //    ValidateLifetime = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(secretkey),
            //};
            //var claims = handler.ValidateToken(token, param, out SecurityToken emailToken);
            return null;
        }
        public ServiceResult<IdentityResultCode> InvalidAccount(Guid accountId)
        {
            return null;
        }

        public void RecoverPassword(string email)
        {
            
        }

        public ServiceResult<IdentityResultCode> ResetPassword(ResetPasswordRequest request)
        {
            return null;
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
            {
                SendActivateEmail(account);
                return BuildResult(true, IdentityResultCode.InactivatedAccount, InactivatedAccountMsg, account);
            }

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
                var isMailSent = SendActivateEmail(newAccount);
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

        private bool SendActivateEmail(Account account)
        {
            var token = GenerateToken(account);
            var loopCounter = 0;
            // If sending email fail then trying 2 more times
            do
            {
                var mailMessage = new MailMessage
                (
                    from: _configuration["OfficalEmailAddress"],
                    to: account.Email,
                    subject: SubjectOfActivateEmail,
                    body: string.Concat(_configuration["AccountActivateAPI"], "?token=", token)

                );
                _mailClient.Send(mailMessage);
                loopCounter++;
                if (loopCounter >= 3)
                    break;
            } while (!_mailClient.MailSent);

            return _mailClient.MailSent;
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

        private SecurityToken GenerateToken(Account account)
        {
            var tokenProperties = _configuration.GetSection("TokenRelated");
            var secretKey = Encoding.ASCII.GetBytes(tokenProperties["Secret"]);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Authentication, "false"),
            };
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = tokenProperties["Issuer"],
                Audience = tokenProperties["Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var emailToken = handler.CreateToken(tokenDescriptor);
            return emailToken;
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
