using Locompro.Repositories;
using Microsoft.Extensions.Logging;

namespace Locompro.Services
{
    /// <summary>
    /// Generic application service.
    /// </summary>
    public class AbstractService
    {
        protected readonly ILogger Logger;
        protected readonly UnitOfWork UnitOfWork;
        
        /// <summary>
        /// Constructs a service.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        protected AbstractService(UnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            UnitOfWork = unitOfWork;
        }
    }
}