using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace BusinessLayer.Helper
{
    public class CustomHasher
    {
        public static string Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool Verify(string input, string hashed)
        {
            return Hash(input) == hashed;
        }
    }
}
