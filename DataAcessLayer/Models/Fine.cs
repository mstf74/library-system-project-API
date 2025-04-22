using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class Fine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Double FineAmount { get; set; }
        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; }
        [Required]
        //[Column(TypeName = "timestamp(6)")]
        [Column(TypeName = "datetime2")]
        public DateTime IssueDate { get; set; }
        [Required]
        [ForeignKey("Loan")]
        public int LoanId { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Loan Loan { get; set; }


    }
}
