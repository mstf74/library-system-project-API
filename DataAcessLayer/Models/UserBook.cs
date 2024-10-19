using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class UserBook
    {
        [ForeignKey("User")]
        public string UserId {  get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public virtual ApplicationUser User {  get; set; }  
    }
}
