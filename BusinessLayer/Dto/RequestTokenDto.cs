using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class RequestTokenDto
    {
        public string jwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
