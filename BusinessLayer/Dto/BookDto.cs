using BusinessLayer.Dto.CustomAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto
{
    public class BookDto
    {
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
        [Range(0, 100)]
        public int NumberOfCopies { get; set; }
        [Required]
        public int AvailableCopies { get; set; }
        [Required]
        public IFormFile  CoverUrl { get; set; }
    }
}
