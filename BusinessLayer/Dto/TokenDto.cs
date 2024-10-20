using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class TokenDto
    {   
        public string token { set; get; }
        public DateTime ExpirDate { get; set; }
        public string UserId { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
