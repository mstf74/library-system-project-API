using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class LoanUserDto
    {
        public string BookName { get; set; }
        public string LoanDate { get; set; }
        public string ReturnDate { get; set; }

    }
    public class LoanAdminDto:LoanUserDto 
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int BookId { get; set; }
        public string Status { get; set; }
    }
}
