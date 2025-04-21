using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class RefreshTokens
    {
        [Key]
        public int Id { get; set; }
        public string token { get; set; } = Guid.NewGuid().ToString();
        public string tokenId { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTime? CreationDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTime? expiryDate { get; set; }
        public bool used { get; set; } = false;
        public bool revoked { get; set; } = false;

        public string userId { get; set; }
        public ApplicationUser? user { get; set; }
    }
}
