using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.Helper
{
    public static class PasswordValidator
    {
        public static (bool IsValid, string ErrorMessage) Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "Password cannot be empty.");
            }

            if (password.Length < 6)
            {
                return (false, "Password must be at least 6 characters long.");
            }

            if (!password.Any(char.IsLetter))
            {
                return (false, "Password must contain at least one letter.");
            }

            if (!password.Any(char.IsDigit))
            {
                return (false, "Password must contain at least one digit.");
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                return (false, "Password must contain at least one special character (e.g. !, @, #, etc.).");
            }

            return (true, string.Empty);
        }
    }
}

    



