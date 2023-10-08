using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;
/// <summary>
/// Provides a context for interacting with the database using entities defined in the Locompro.Models namespace.
/// It extends the IdentityDbContext with a custom user entity.
/// </summary>
public class LocomproContext : IdentityDbContext<User>
{
    /// <summary>
    /// Gets or sets the set of countries.
    /// </summary>
    public DbSet<Country> Countries { get; set; } = default!;

    /// <summary>
    /// Gets or sets the set of provinces.
    /// </summary>
    public DbSet<Province> Provinces { get; set; } = default!;

    /// <summary>
    /// Gets or sets the set of cantons.
    /// </summary>
    public DbSet<Canton> Cantons { get; set; } = default!;

    /// <summary>
    /// Gets or sets the set of categories.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = default!;

    /// <summary>
    /// Gets or sets the set of submissions.
    /// </summary>
    public DbSet<Submission> Submissions { get; set; } = default!;

    /// <summary>
    /// Gets or sets the set of stores.
    /// </summary>
    public DbSet<Store> Stores { get; set; } = default!;

    /// <summary>
    /// Gets or sets the set of products.
    /// </summary>
    public DbSet<Product> Products { get; set; } = default!;
    /// <summary>
    /// Initializes a new instance of the <see cref="LocomproContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public LocomproContext(DbContextOptions<LocomproContext> options)
        : base(options)
    {
    }
    /// <summary>
    /// Configures the schema needed for the context.
    /// </summary>
    /// <param name="builder">The builder being used to construct the context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Province>()
            .HasKey(p => new { p.CountryName, p.Name });

        builder.Entity<Province>()
            .HasOne(p => p.Country)
            .WithMany(c => c.Provinces)
            .HasForeignKey(p => p.CountryName)
            .IsRequired();
        
        builder.Entity<Canton>()
            .HasKey(c => new { c.CountryName, c.ProvinceName, c.Name });

        builder.Entity<Canton>()
            .HasOne(c => c.Province)
            .WithMany(p => p.Cantons)
            .HasForeignKey(c => new { c.CountryName, c.ProvinceName })
            .IsRequired();

        builder.Entity<Category>()
            .HasKey(c => new { c.Name });
        
        builder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children);

        builder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products);
        
        builder.Entity<Submission>()
            .HasKey(s => new { s.Username, s.EntryTime });

        builder.Entity<Submission>()
            .HasOne(s => s.Store)
            .WithMany()
            .HasForeignKey(s => s.StoreName)
            .IsRequired();
        
        builder.Entity<Submission>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Submissions)
            .HasForeignKey(s => s.ProductId)
            .IsRequired();
        
        builder.Entity<Submission>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.Username)
            .IsRequired();

        builder.Entity<User>()
            .HasMany(u => u.Submissions)
            .WithOne(s => s.User);
    }
}