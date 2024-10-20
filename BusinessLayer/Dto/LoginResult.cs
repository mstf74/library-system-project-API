using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? ErrorMessage { get; set; }
        public string Id { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
