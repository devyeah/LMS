using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DevYeah.LMS.Business.Helpers
{
    public class Identityhelper
    {
        public static string GenerateToken(string key, string issuer, string audience, Claim[] claims, DateTime expires)
        {
            var secretKey = Encoding.ASCII.GetBytes(key);

            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Audience = audience,
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var emailToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(emailToken);
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return null;

            string hashedPassword = null;
            using (var md5Hash = MD5.Create())
            using (var sha256 = SHA256.Create())
            {
                var md5Password = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sha256Password = sha256.ComputeHash(md5Password);
                hashedPassword = BuildHexadecimalString(sha256Password);
            }
            return hashedPassword;
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
