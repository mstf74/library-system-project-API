using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class TokenResultDto
    {   
        public string token { set; get; }
        public string refreshToken { set; get; }
        public string id { set; get; }
        public string role { set; get; }

    }
}
