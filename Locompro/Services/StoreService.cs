using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services
{
    /// <summary>
    /// Service for Store entities.
    /// </summary>
    public class StoreService : AbstractDomainService<Store, string, StoreRepository>
    {
        public StoreService(UnitOfWork unitOfWork, StoreRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
