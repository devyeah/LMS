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
        private static readonly string ActivateMailSendFail = "Because some unknown reason, your sign up failed. Please try again later.";
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
            //if (request == null ||
            //    string.IsNullOrWhiteSpace(request.Email) ||
            //    string.IsNullOrWhiteSpace(request.Password))
            //    return new ServiceResult<IdentityResultCode>
            //    {
            //        IsSuccess = false,
            //        ResultCode = IdentityResultCode.SignInFailure,
            //        Message = "The necessary information is incomplete.",
            //    };

            //var account = _repository.GetUniqueAccountByEmail(request.Email);
            //if (account == null)
            //    return new ServiceResult<IdentityResultCode>
            //    {
            //        IsSuccess = false,
            //        ResultCode = IdentityResultCode.EmailError,
            //        Message = "This user is not exist.",
            //    };

            ////Todo: check wether or not the current user has been activated, if not sending a email
            //if (account.Status == (int)AccountStatus.Inactivated)
            //{
            //    var emailToken = GenerateToken(account);
            //    _smtpClient.SendMailAsync(new MailMessage(
            //            to: account.Email,
            //            from: "yyy@mail.com",
            //            subject: "Test message subject",
            //            body: string.Concat(_configuration["ActivateLink"], "?token=", emailToken.ToString())
            //            ));
            //    return new ServiceResult<IdentityResultCode>
            //    {
            //        IsSuccess = false,
            //        ResultCode = IdentityResultCode.InactivatedAccount,
            //        Message = "You need to activate your account before signing in the website."
            //    };
            //}

            //var md5HashedPws = GetMd5Hash(request.Password);
            //var sha256Pws = GetSha256Hash(md5HashedPws);
            //if (account.Password.Equals(sha256Pws))
            //    return new ServiceResult<IdentityResultCode>
            //    {
            //        IsSuccess = true,
            //        ResultCode = IdentityResultCode.Success,
            //        Message = "",
            //        ResultObj = account,
            //    };

            //return new ServiceResult<IdentityResultCode>
            //{
            //    IsSuccess = false,
            //    ResultCode = IdentityResultCode.PasswordError,
            //    Message = "Password is not correct.",
            //};
            return null;
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
                    return BuildResult(false, IdentityResultCode.EmailError, ActivateMailSendFail);
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
            do
            {
                var mailMessage = new MailMessage
                (
                    from: _configuration["OfficalEmailAddress"],
                    to: account.Email,
                    subject: "Thank you for signing up, Please activate your account",
                    body: string.Concat(_configuration["AccountActivateAPI"], "?token=", token)

                );
                _mailClient.Send(mailMessage);
                loopCounter++;
                if (loopCounter >= 3)
                    break;
            } while (!_mailClient.MailSent);

            return _mailClient.MailSent;
        }

        private static ServiceResult<IdentityResultCode> BuildResult(bool isSuccess, IdentityResultCode code, string message, object resultObj = null)
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
