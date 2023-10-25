using Locompro.Models;
using Microsoft.EntityFrameworkCore;

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
        public CategoryRepository(DbContext context, ILoggerFactory loggerFactory)
            : base(context, loggerFactory)
        {
        }
    }
}