using Locompro.Data;

namespace Locompro.Services
{
    /// <summary>
    /// Generic application service.
    /// </summary>
    public abstract class AbstractService
    {
        protected readonly ILogger Logger;
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructs a service.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        protected AbstractService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            UnitOfWork = unitOfWork;
        }
    }
}