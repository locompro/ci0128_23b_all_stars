using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

public class LocomproContext : IdentityDbContext<User>
{
    public DbSet<Store> Store { get; set; } = default!;
    public DbSet<Country> Country { get; set; } = default!;
    public DbSet<Province> Province { get; set; } = default!;
    public DbSet<Canton> Canton { get; set; } = default!;

    public DbSet<Category> Category { get; set; } = default!;

    public LocomproContext(DbContextOptions<LocomproContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Province
        builder.Entity<Province>()
            .HasKey(p => new { p.CountryName, p.Name });

        builder.Entity<Province>()
            .HasOne(p => p.Country)
            .WithMany(c => c.Provinces)
            .HasForeignKey(p => p.CountryName);

        // Canton
        builder.Entity<Canton>()
            .HasKey(c => new { c.CountryName, c.ProvinceName, c.Name });

        builder.Entity<Canton>()
            .HasOne(c => c.Province)
            .WithMany(p => p.Cantons)
            .HasForeignKey(c => new { c.CountryName, c.ProvinceName });

        builder.Entity<Category>()
            .HasKey(c => new { c.Name });
    }
}