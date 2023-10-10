using Locompro.Data;
using Locompro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories
{
    /// <summary>
    /// Repository for products
    /// </summary>
    public class ProductRepository : AbstractRepository<Product, int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public ProductRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
        
        public async Task<IEnumerable<Product>> GetByPartialNameAsync(string partialName)
        {
            return await DbSet.Where(e => e.Name.Contains(partialName)).ToListAsync();
        }
    }
}
