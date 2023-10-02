using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Locompro.Data
{
    public class LocomproContext : IdentityDbContext<User>
    {
        public LocomproContext(DbContextOptions<LocomproContext> options)
            : base(options)
        {
        }

        public DbSet<Locompro.Models.Store> Store { get; set; } = default!;
        public DbSet<Locompro.Models.Province> Province { get; set; } = default!;
        public DbSet<Locompro.Models.Country> Country { get; set; } = default!;
        public DbSet<Locompro.Models.Canton> Canton { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<Province>().ToTable("Province");
            modelBuilder.Entity<Canton>().ToTable("Canton");
            base.OnModelCreating(modelBuilder);
        }
    }
}
