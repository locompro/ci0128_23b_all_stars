using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Domain service for Store entities.
    /// </summary>
    public class StoreService : NamedEntityDomainService<Store, string>
    {
        /// <summary>
        /// Constructs a Store service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public StoreService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
        {
        }
    }
}
