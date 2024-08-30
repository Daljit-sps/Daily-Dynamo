using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Models.DTO;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static DailyDynamo.Shared.Utilities.AppSettingsUtility;

namespace DailyDynamo.Shared.ExtensionMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// to read content from HTML file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadHtmlContentFromFile(string filePath)
        {
            using var streamReader = new StreamReader(filePath);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// to convert default password into hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using(var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt using bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, Settings.Password.Salt);

            return hashedPassword;
        }


        public static bool VerifyHash(string password, string passwordHash)
        {
            string hashedPassword = HashPassword(password);
            return string.Equals(hashedPassword, passwordHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// to encode the email links
        /// </summary>
        /// <param name="pageLink"></param>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string EncodeEmailLinkAsync(string pageLink, Guid userId, string email, bool isCheckAdded)
        {
            var encodedUserId = WebUtility.UrlEncode(userId.ToString());
            var encodedEmail = WebUtility.UrlEncode(email);
            var encodedCheck = isCheckAdded ? "1" : "0";

            // Construct the encoded email verification link
            var encodedVerificationLink = $"{pageLink}?token={encodedUserId}&email={encodedEmail}&isCheckAdded={encodedCheck}";

            return encodedVerificationLink;
        }

        /// <summary>
        /// validate email and mobile number
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public static (bool, string) ValidateSignUpModel(SignUpViewModel newUser)
        {
            string emailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            string mobileRegexPattern = @"^\d{10}$";
            if(!Regex.IsMatch(newUser.EmailId, emailRegexPattern))
                return (false, "invalid email format");
            if(!Regex.IsMatch(newUser.MobileNo, mobileRegexPattern))
                return (false, "invalid mobile format");

            return (true, string.Empty);
        }

        public static string GenerateRandomFileName(string fileName)
        {
            return string.Concat("DD_", Guid.NewGuid().ToString().Replace("-", ""), fileName.Substring(fileName.LastIndexOf("."))).ToString().ToLower();
        }


    }
}
