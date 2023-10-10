using Locompro.Data;
using Locompro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories
{
    /// <summary>
    /// Repository for Store entities.
    /// </summary>
    public class StoreRepository : StringIdRepository<Store>
    {
        /// <summary>
        /// Constructs a Store repository for a given context.
        /// </summary>
        /// <param name="context">Context to base the repository on.</param>
        /// <param name="loggerFactory">Factory for repository logger.</param>
        public StoreRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
    }
}
