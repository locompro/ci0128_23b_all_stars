using Locompro.Models;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories
{
    /// <summary>
    /// Repository for products
    /// </summary>
    public class ProductRepository : CrudRepository<Product, int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="loggerFactory"></param>
        public ProductRepository(DbContext dbContext, ILoggerFactory loggerFactory) : base(dbContext, loggerFactory)
        {
        }

        public async Task<IEnumerable<Product>> GetByPartialNameAsync(string partialName)
        {
            return await Set.Where(e => e.Name.Contains(partialName)).ToListAsync();
        }
    }
}