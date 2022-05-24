using AtonWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AtonTestTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Login).IsRequired();
        
        }
        public DbSet<User> Users { get; set; }
    }
}