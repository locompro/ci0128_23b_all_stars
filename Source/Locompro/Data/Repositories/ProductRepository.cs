using Locompro.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

public class ProductRepository : CrudRepository<Product, int>, IProductRepository
{
    public ProductRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }

    public async Task<List<string>> GetBrandsAsync()
    {
        return await Set.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<List<string>> GetModelsAsync()
    {
        return await Set.Select(p => p.Model).Distinct().ToListAsync();
    }
}