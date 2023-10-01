using Locompro.Repositories;

namespace Locompro.Services
{
    /// <summary>
    /// Abstract class representing services.
    /// </summary>
    public class AbstractService
    {
        protected readonly UnitOfWork unitOfWork;
        
        protected AbstractService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}