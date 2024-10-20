using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in entities)
            {
                try
                {
                    ValidationContext validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }
                catch (Exception ex) 
                {
                    throw new Exception(message: ex.Message);
                }
            }

            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

        
    }
}

