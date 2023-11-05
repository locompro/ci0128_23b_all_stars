using System.Reflection;
using Castle.Core.Internal;
using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

/// <summary>
/// ORM interface for Locompro.
/// </summary>
public class LocomproContext : IdentityDbContext<User>
{
    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Province> Provinces { get; set; } = default!;
    public DbSet<Canton> Cantons { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Submission> Submissions { get; set; } = default!;
    public DbSet<Store> Stores { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Picture> Pictures { get; set; } = default!;

    /// <summary>
    /// Constructs a Locompro context.
    /// </summary>
    /// <param name="options">Context creation options.</param>
    public LocomproContext(DbContextOptions<LocomproContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
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
            .HasKey(s => new { Username = s.UserId, s.EntryTime });

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
            .HasForeignKey(s => s.UserId)
            .IsRequired();

        builder.Entity<User>()
            .HasMany(u => u.Submissions)
            .WithOne(s => s.User);

        builder.Entity<Picture>()
            .HasKey(p => new { p.SubmissionUserId, p.SubmissionEntryTime, p.Index });

        builder.Entity<Picture>()
            .HasOne<Submission>(p => p.Submission)
            .WithMany(s => s.Pictures)
            .HasForeignKey(p => new { p.SubmissionUserId, p.SubmissionEntryTime })
            .IsRequired();
        builder.Entity<GetPicturesResult>().HasNoKey();

        builder.HasDbFunction(
            typeof(LocomproContext).GetMethod(nameof(GetQualifiedUserIDs), BindingFlags.Static) ??
            throw new InvalidOperationException($"Method {nameof(GetQualifiedUserIDs)} not found."));
        builder.HasDbFunction(
            typeof(LocomproContext).GetMethod(nameof(CountRatedSubmissions), BindingFlags.Static) ??
            throw new InvalidOperationException($"Method {nameof(CountRatedSubmissions)} not found."));
        builder.HasDbFunction(
            typeof(LocomproContext).GetMethod(nameof(GetPictures), BindingFlags.Static) ??
            throw new InvalidOperationException($"Method {nameof(GetPictures)} not found."));
    }

    [DbFunction("GetPictures", "dbo")]
    public static IQueryable<GetPicturesResult> GetPictures(string storeName, int productId, int maxPictures)
    {
        throw new NotSupportedException();
    }

    [DbFunction("CountRatedSubmissions", "dbo")]
    public static int CountRatedSubmissions(string userId)
    {
        throw new NotSupportedException();
    }

    [DbFunction("GetQualifiedUserIDs", "dbo")]
    public static IQueryable<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Assigns each parent category of a product to the product.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <param name="productId"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual async Task AddParents(string categoryName, int productId)
    {
        if (categoryName.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(categoryName));

        var categoryNameParameter = new SqlParameter("@category", categoryName);
        var productIdParameter = new SqlParameter("@productId", productId);

        await Database.ExecuteSqlRawAsync("EXECUTE dbo.AddParents @category, @productId", categoryNameParameter,
            productIdParameter);
    }

    /// <summary>
    /// Deletes every submission that has been deemed inappropriate by a moderator.
    /// </summary>
    public virtual async Task DeleteModeratedSubmissions()
    {
        await Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteModeratedSubmissions");
    }
}