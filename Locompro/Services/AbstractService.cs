using Locompro.Repositories;

namespace Locompro.Services
{
    /// <summary>
    /// Generic application service.
    /// </summary>
    public class AbstractService
    {
        protected readonly UnitOfWork unitOfWork;
        
        /// <summary>
        /// Constructs a service.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        protected AbstractService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}