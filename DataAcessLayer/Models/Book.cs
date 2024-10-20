
using DataAcessLayer.Models.CustomAttributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]

        public string Author { get; set; }
        [Required] 
        public string publicationYear { get; set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; set; }
        [Required]
        [Range(0,100)]
        public int NumberOfCopies { get; set; }
        [Required]
        [LessCopies]
        public int AvailableCopies{ get; set; }
        public string? CoverUrl { get; set; }
        public virtual ICollection<UserBook> Users{ get;set; } = new List<UserBook>();
        public virtual ICollection<Loan> Loans{ get;set; } = new List<Loan>();

    }

}
