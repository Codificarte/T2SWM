using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace T2SLogistics.Helpers
{
    public static class ValidationHelper
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex pattern for validating email
            var emailRegex = EmailPattern;
            return Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase);
        }
    }
}
