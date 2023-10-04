using Locompro.Data;
using Locompro.Models;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories
{
    /// <summary>
    /// Repository for categories
    /// </summary>
    public class CategoryRepository : AbstractRepository<Category, string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public CategoryRepository(LocomproContext context, ILoggerFactory loggerFactory)
            : base(context, loggerFactory)
        {
        }
    }
}