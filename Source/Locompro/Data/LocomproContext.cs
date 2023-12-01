using Castle.Core.Internal;
using Locompro.Models.Entities;
using Locompro.Models.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

/// <summary>
///     ORM interface for Locompro.
/// </summary>
public class LocomproContext : IdentityDbContext<User>
{
    /// <summary>
    ///     Constructs a Locompro context.
    /// </summary>
    /// <param name="options">Context creation options.</param>
    public LocomproContext(DbContextOptions<LocomproContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Province> Provinces { get; set; } = default!;
    public DbSet<Canton> Cantons { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Submission> Submissions { get; set; } = default!;
    public DbSet<Report> Reports { get; set; } = default!;
    public DbSet<UserReport> UserReports { get; set; } = default!;
    public DbSet<AutoReport> AutoReports { get; set; } = default!;
    public DbSet<Store> Stores { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Picture> Pictures { get; set; } = default!;

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
            .HasKey(s => new { s.UserId, s.EntryTime });

        builder.Entity<Submission>()
            .HasOne(s => s.Store)
            .WithMany()
            .HasForeignKey(s => s.StoreName)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Entity<Submission>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Submissions)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Entity<Submission>()
            .HasOne(s => s.User)
            .WithMany(u => u.CreatedSubmissions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Entity<Submission>()
            .HasMany(s => s.Approvers)
            .WithMany(u => u.ApprovedSubmissions)
            .UsingEntity<Dictionary<string, object>>(
                "SubmissionApprover", // Name of the join table for approvers
                j => j.HasOne<User>().WithMany().HasForeignKey("ApproverId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Submission>().WithMany().HasForeignKey("SubmissionUserId", "SubmissionEntryTime").OnDelete(DeleteBehavior.Cascade)
            );

        builder.Entity<Submission>()
            .HasMany(s => s.Rejecters)
            .WithMany(u => u.RejectedSubmissions)
            .UsingEntity<Dictionary<string, object>>(
                "SubmissionRejecter", // Name of the join table for rejecters
                j => j.HasOne<User>().WithMany().HasForeignKey("RejecterId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Submission>().WithMany().HasForeignKey("SubmissionUserId", "SubmissionEntryTime").OnDelete(DeleteBehavior.Cascade)
            );

        builder.Entity<Submission>()
            .HasMany(s => s.UserReports)
            .WithOne(r => r.Submission)
            .IsRequired();

        builder.Entity<User>()
            .HasMany(u => u.CreatedSubmissions)
            .WithOne(s => s.User)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Report>()
            .HasKey(r => new { r.SubmissionUserId, r.SubmissionEntryTime, r.UserId });

        builder.Entity<UserReport>().HasBaseType<Report>();

        builder.Entity<UserReport>()
            .HasOne(r => r.Submission)
            .WithMany(s => s.UserReports)
            .HasForeignKey(r => new { r.SubmissionUserId, r.SubmissionEntryTime })
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Entity<UserReport>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .IsRequired();

        builder.Entity<AutoReport>().HasBaseType<Report>();

        builder.Entity<AutoReport>()
            .HasOne(r => r.Submission)
            .WithMany(s => s.AutoReports)
            .HasForeignKey(r => new { r.SubmissionUserId, r.SubmissionEntryTime })
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Entity<AutoReport>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .IsRequired();

        builder.Entity<Picture>()
            .HasKey(p => new { p.SubmissionUserId, p.SubmissionEntryTime, p.Index });

        builder.Entity<Picture>()
            .HasOne(p => p.Submission)
            .WithMany(s => s.Pictures)
            .HasForeignKey(p => new { p.SubmissionUserId, p.SubmissionEntryTime })
            .IsRequired();
        builder.Entity<GetPicturesResult>().HasNoKey();

        builder.HasDbFunction(
            typeof(LocomproContext).GetMethod(nameof(GetQualifiedUserIDs)) ??
            throw new InvalidOperationException($"Method {nameof(GetQualifiedUserIDs)} not found."));
        builder.HasDbFunction(
            typeof(LocomproContext).GetMethod(nameof(CountRatedSubmissions), new[] { typeof(string) }) ??
            throw new InvalidOperationException($"Method {nameof(CountRatedSubmissions)} not found."));
        builder.HasDbFunction(
            typeof(LocomproContext).GetMethod(nameof(GetPictures),
                new[] { typeof(string), typeof(int), typeof(int) }) ??
            throw new InvalidOperationException($"Method {nameof(GetPictures)} not found."));
    }

    [DbFunction("GetPictures", "dbo")]
    public IQueryable<GetPicturesResult> GetPictures(string storeName, int productId, int maxPictures)
    {
        return FromExpression(() => GetPictures(storeName, productId, maxPictures));
    }

    [DbFunction("CountRatedSubmissions", "dbo")]
    public int CountRatedSubmissions(string userId)
    {
        throw new NotSupportedException();
    }

    [DbFunction("GetQualifiedUserIDs", "dbo")]
    public IQueryable<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        return FromExpression(() => GetQualifiedUserIDs());
    }

    /// <summary>
    ///     Assigns each parent category of a product to the product.
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
    ///     Deletes every submission that has been deemed inappropriate by a moderator.
    /// </summary>
    public virtual async Task DeleteModeratedSubmissions()
    {
        await Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteModeratedSubmissions");
    }
}