using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
     public class Context: IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options) :base(options) 
        {
        }
        public virtual DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBook>().HasKey(e => new { e.UserId, e.BookId });
            modelBuilder.Entity<UserBook>()
                .HasOne(e => e.User)
                .WithMany(u => u.BorrowoedBooks)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<UserBook>()
                .HasOne(e => e.Book)
                .WithMany(b=>b.Users)
                .HasForeignKey(e=>e.BookId);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Fine>()
                .HasOne(f => f.Loan)
                .WithOne(l => l.Fine)
                .HasForeignKey<Fine>(f => f.LoanId);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

        
    }
}

