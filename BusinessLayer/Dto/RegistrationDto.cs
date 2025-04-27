using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class AccountDto
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string email { get; set; }
        [Required]
        public string user_name { get; set; }
        [Required]
        public string phoneNumber { get; set; }

    }
    public class RegistrationDto:AccountDto
    {
        
        [Required]
        public string password { get; set; }

    }
}
