using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WalkingForRochester.Controls
{
    public static class ToolKit
    {
        /// <summary>
        ///     Using MD5 encrypt to password
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encrypt32Bit(string s)
        {
            var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        ///     Using Regular Expression to validate user input the date of birth
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        public static bool ValidateDateOfBirth(DateTime dob)
        {
            return Regex.IsMatch(dob.ToShortDateString(), @"^\d{1,2}/\d{1,2}/\d{4}$");
        }

        /// <summary>
        ///     Calculate age from user input the date of birth
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        public static bool Over18Age(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            return age >= 18;
        }

        /// <summary>
        ///     Using Regular Expression to validate user input email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
    }
}