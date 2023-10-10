using Locompro.Data;
using Locompro.Models;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories
{
    /// <summary>
    /// Repository for Store entities.
    /// </summary>
    public class StoreRepository : AbstractRepository<Store, string>
    {
        /// <summary>
        /// Constructs a Store repository for a given context.
        /// </summary>
        /// <param name="context">Context to base the repository on.</param>
        /// <param name="loggerFactory">Factory for repository logger.</param>
        public StoreRepository(LocomproContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
    }
}
