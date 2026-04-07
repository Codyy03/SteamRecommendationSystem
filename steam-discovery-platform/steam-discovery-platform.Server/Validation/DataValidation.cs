using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace steam_discovery_platform.Server.Validation
{
    public class DataValidation
    {
        [NonAction]
        public List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                errors.Add("Password must be at least 8 characters long.");

            return errors;
        }

        [NonAction]
        public List<string> ValidateName(string name)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Name is required.");
                return errors;
            }
            return errors;
        }

        [NonAction]
        public List<string> ValidateEmail(string email)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("email is required.");
                return errors;
            }

            if (!email.Any(ch => "@".Contains(ch)))
                errors.Add("email must contain @");

            return errors;
        }
    }
}
