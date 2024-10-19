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
    }
}
