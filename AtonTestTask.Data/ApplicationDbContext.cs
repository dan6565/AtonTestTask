using AtonWebApi.Entities;
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
            modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
        
        }
        public DbSet<User> Users { get; set; }
    }
}