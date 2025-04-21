using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "timestamp(6)")]
        public DateTime LoanDate { get; set; }
        [Required]
        [Column(TypeName = "timestamp(6)")]
        public DateTime DueDate { get; set; }
        [Required]
        [Column(TypeName = "timestamp(6)")]
        public DateTime ReturnDate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [ForeignKey("User")]
        public String UserId { get; set; }
        [Required]
        [ForeignKey("Book")]
        public int BookId {  get; set; } 

        public virtual ApplicationUser User { get; set; }
        public virtual Book Book { get; set; }
        public virtual Fine Fine { get; set; }
    }
}
