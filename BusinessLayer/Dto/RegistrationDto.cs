using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class RegistrationDto
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string user_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string password { get; set; }

    }
}
