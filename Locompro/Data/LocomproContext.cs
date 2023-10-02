using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data
{
    public class LocomproContext : IdentityDbContext<User>
    {
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Canton> Cantons { get; set; }
        
        public LocomproContext(DbContextOptions<LocomproContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Province
            modelBuilder.Entity<Province>()
                .HasKey(p => new { p.CountryName, p.Name });

            modelBuilder.Entity<Province>()
                .HasOne(p => p.Country)
                .WithMany(c => c.Provinces)
                .HasForeignKey(p => p.CountryName);
            
            // Canton
            modelBuilder.Entity<Canton>()
                .HasKey(c => new { c.CountryName, c.ProvinceName, c.Name });

            modelBuilder.Entity<Canton>()
                .HasOne(c => c.Province)
                .WithMany(p => p.Cantons)
                .HasForeignKey(c => new { c.CountryName, c.ProvinceName });
        }
    }
}
