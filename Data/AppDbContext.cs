using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PMS.Models;

namespace PMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<AppUser>()
             .HasIndex(u => u.Email)
             .IsUnique();
            


        }


        public DbSet<AppUser> AppUsers { get; set; }
                   
    }

  }

        
        
        
    
