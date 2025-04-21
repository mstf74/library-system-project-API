using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "timestamp(6)")]
        public DateTime MemberShipDate { get; set; }
        [DefaultValue("user")]
        public String Role { get; set; } = "User";
        public virtual ICollection<UserBook> BorrowoedBooks { get; set; } = new List<UserBook>();
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();


    }
}
