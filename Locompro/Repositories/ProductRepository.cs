using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        /// Returns all products that have the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> getProductsByName(string name)
        {
            // get all products with the same name
            IQueryable<Product> productsQuery = this.DbSet.Where(p => p.Name.Contains(name));

            return await productsQuery.ToListAsync();
        }
        
        /// <summary>
        /// Asyncronously gets products by model
        /// </summary>
        /// <param name="model"></param>
        public async Task<IEnumerable<Product>> getByModelAsync(string model)
        {
            var products = await DbSet.Where(p => p.Model.Contains(model)).ToListAsync();
            
            return products;
        }
        
    }
}
