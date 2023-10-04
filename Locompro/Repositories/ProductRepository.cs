using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    /// <summary>
    /// Repository for products
    /// </summary>
    public class ProductRepository : AbstractRepository<Product, string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public ProductRepository(LocomproContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
        
        /// <summary>
        /// Asyncronously gets products by model
        /// </summary>
        /// <param name="model"></param>
        public async Task<IEnumerable<Product>> getByModelAsync(string model)
        {
            var products = await DbSet.Where(p => p.Model == model).ToListAsync();
            
            return products;
        }
        
    }
}
