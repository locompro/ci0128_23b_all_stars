using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services
{
    /// <summary>
    /// Domain service for Store entities.
    /// </summary>
    public class StoreService : AbstractDomainService<Store, string, StoreRepository>
    {
        /// <summary>
        /// Constructs a Store service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        public StoreService(UnitOfWork unitOfWork, StoreRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
