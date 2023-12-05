using Locompro.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

public struct SubmissionKey
{
    public string UserId { get; set; }
    public DateTime EntryTime { get; set; }
}

/// <summary>
///     Repository for Submission entities.
///     Key is a tuple of the country name and the Datetime of the submission.
/// </summary>
public class SubmissionRepository : CrudRepository<Submission, SubmissionKey>, ISubmissionRepository
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="loggerFactory"></param>
    public SubmissionRepository(DbContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory)
    {
    }

    /// <inheritdoc />
    public override async Task<Submission> GetByIdAsync(SubmissionKey id)
    {
        Submission submission = await Set.FirstOrDefaultAsync(submission =>
            submission.UserId == id.UserId &&
            submission.EntryTime == id.EntryTime);

        if (submission == null)
        {
            throw new InvalidOperationException("Error loading submission! No submission for user:" + id.UserId +
                                                " and entry time: " +
                                                id.EntryTime + " was found.");
        }

        return submission;
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(SubmissionKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null) Set.Remove(entity);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetByUserIdAsync(string userId)
    {
        return await Set.Where(e => e.UserId == userId).ToListAsync();
    }

    public async Task<Submission> GetByIdAsync(string userId, DateTime entryTime)
    {
        return await Set.FindAsync(userId, entryTime);
    }

    public async Task UpdateAsync(string userId, DateTime entryTime, Submission entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var existingEntity = await GetByIdAsync(userId, entryTime);
        if (existingEntity != null)
        {
            Context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();
        }
        else
        {
            await AddAsync(entity);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName)
    {
        var submissionsResults = Set
            .Include(submission => submission.Product)
            .Where(submission => submission.Product.Name == productName && submission.Store.Name == storeName);

        return await submissionsResults.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductSummaryStore>> GetProductSummaryByStore(List<int> productIds)
    {
        int totalProductCount = productIds.Count;

        var storeProductCounts = await Set
            .Include(s => s.Product)
            .Include(s => s.Store)
            .ThenInclude(store => store.Canton)
            .Where(s => productIds.Contains(s.ProductId))
            .Select(s => new
            {
                s.Store.Name,
                Province = s.Store.Canton.Province,
                s.Store.Canton,
                s.ProductId,
                s.Price,
                s.EntryTime
            })
            .ToListAsync();

        var groupedData = storeProductCounts
            .GroupBy(s => new { s.Name, s.Province, s.Canton })
            .Select(group => new ProductSummaryStore
            {
                Name = group.Key.Name,
                Province = group.Key.Province,
                Canton = group.Key.Canton,
                ProductsAvailable = group.Select(g => g.ProductId).Distinct().Count(),
                PercentageProductsAvailable = 
                    (int)(group.Select(g => g.ProductId).Distinct().Count() / (float)totalProductCount * 100),
                TotalCost = group
                    .GroupBy(g => g.ProductId)
                    .Select(g => g.OrderByDescending(sub => sub.EntryTime).FirstOrDefault())
                    .Sum(sub => sub.Price)
            }).ToList();

        return groupedData;
    }
}