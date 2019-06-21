using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Helpers;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LMSAccount = DevYeah.LMS.Models.Account;
using CloudinaryAccount = CloudinaryDotNet.Account;

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
        private readonly static string EmptyFileErrorMsg = "No file has been upload in request.";
        private readonly static string ImageUploadFailMsg = "Uploading of image is failed";

        private readonly IAccountRepository _repository;
        private readonly IEmailClient _mailClient;
        private readonly TokenSettings _tokenSettings;
        private readonly ApiSettings _apiSettings;
        private readonly EmailTemplate _emailTemplate;
        private readonly HostEnvironment _hostEnvironment;
        private readonly CloudinarySettings _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public AccountService(IAccountRepository repository, IEmailClient mailClient,
            IOptions<TokenSettings> tokenSettings, IOptions<ApiSettings> apiSettings,
            IOptions<EmailTemplate> emailTemplate, IOptions<HostEnvironment> hostEnvironment,
            IOptions<CloudinarySettings> cloudinarySettings)
        {
            _repository = repository;
            _mailClient = mailClient;
            _tokenSettings = tokenSettings.Value;
            _apiSettings = apiSettings.Value;
            _emailTemplate = emailTemplate.Value;
            _hostEnvironment = hostEnvironment.Value;
            _cloudinarySettings = cloudinarySettings.Value;

            var cloudinaryAccount = new CloudinaryAccount(_cloudinarySettings.CloudName, 
                _cloudinarySettings.APIKey, _cloudinarySettings.APISecret);
            _cloudinary = new Cloudinary(cloudinaryAccount);
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

            var subject = _emailTemplate.PasswordRecoveryMailSubject;
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
            var isRequestValid = ValidateSignInRequest(request);
            if (!isRequestValid)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, ArgumentNullMsg);

            var account = _repository.GetUniqueAccountByEmail(request.Email);
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
            var newAccount = new LMSAccount
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
                _repository.Add(newAccount);
                _repository.SaveChanges();
                var subject = _emailTemplate.SignUpMailSubject;
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

        public ServiceResult<IdentityResultCode> UploadImage(UploadImageRequest request)
        {
            var image = request.Files.FirstOrDefault();
            if (image == null || image.Length == 0)
                return BuildResult(false, IdentityResultCode.IncompleteArgument, EmptyFileErrorMsg);

            var localPath = SaveImage(image, out string imageName);
            if (imageName == null)
                return BuildResult(false, IdentityResultCode.SaveImageFailure, ImageUploadFailMsg);

            var uploadResult = RetryUpload(() => UploadImage(localPath, imageName), 3);
            if (uploadResult == null)
                return BuildResult(false, IdentityResultCode.UploadImageFailure, ImageUploadFailMsg);

            return BuildResult(true, IdentityResultCode.Success, resultObj: uploadResult.JsonObj);
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

        private string BuildAccountActivationMail(LMSAccount account)
        {
            var token = GenerateAccountActivationToken(account);
            var link = string.Concat(_apiSettings.AccountActivationAPI, "?token=", token);
            var templateKey = nameof(_emailTemplate.SignUpMailContent);
            var template = _emailTemplate.SignUpMailContent;
            var content = RenderedEmailHelper.Parse(templateKey, template, new TemplateModel { Link = link });
            return content;
        }

        private string BuildPasswordRecoveryMail(LMSAccount account)
        {
            var token = GeneratePasswordRecoveryToken(account.Email);
            var link = string.Concat(_apiSettings.PasswordRecoveryAPI, "?token=", token);
            var templateKey = nameof(_emailTemplate.PasswordRecoveryMailContent);
            var template = _emailTemplate.PasswordRecoveryMailContent;
            var content = RenderedEmailHelper.Parse(templateKey, template, new TemplateModel { Link = link });
            return content;
        }

        private string GenerateAuthenticatedToken(LMSAccount account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
            };
            
            return MakeToken(claims);
        }

        private string GenerateAccountActivationToken(LMSAccount account)
        {
            var claims = new []
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
            return IdentityHelper.GenerateToken(_tokenSettings.Secret, _tokenSettings.Issuer,
                _tokenSettings.Audience, claims, _tokenSettings.Expires);
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

        private ImageUploadResult UploadImage(string path, string name)
        {
            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(path),
                PublicId = $"{_cloudinarySettings.AvatarFolder}/{name}"
            };
            return _cloudinary.Upload(uploadParam);
        }

        private string SaveImage(IFormFile image, out string imageName)
        {
            imageName = null;
            try
            {
                var suffix = Path.GetExtension(image.FileName);
                imageName = $"{Guid.NewGuid().ToString()}{suffix}";
                Directory.CreateDirectory(_hostEnvironment.ImageFolder);
                var filePath = Path.Combine(_hostEnvironment.ImageFolder, imageName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                return filePath;
            }
            catch (Exception)
            {
                // Log exceptions
                return null;
            }
        }

        private ImageUploadResult RetryUpload(Func<ImageUploadResult> logic, int maxRetryCounter, Action logImportant = null, Action logError = null)
        {
            var loopCounter = 0;
            // If uploading image fail then trying another 2 times
            do
            {
                loopCounter++;
                try
                {
                    var result = logic?.Invoke();
                    if (result != null)
                        return result;
                }
                catch (Exception)
                {
                    logImportant?.Invoke();
                    Thread.Sleep(1000);
                }
            } while (loopCounter < maxRetryCounter);
            if (loopCounter > 1)
                logError?.Invoke();
            return null;
        }
    }
}
