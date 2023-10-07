using Locompro.Data;
using Locompro.Models;
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
        
    }
}
