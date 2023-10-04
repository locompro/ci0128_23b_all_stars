using Locompro.Models;
using Locompro.Repositories;
using Microsoft.Extensions.Logging;

namespace Locompro.Services
{
    /// <summary>
    /// Domain service for User entities.
    /// </summary>
    public class UserService : AbstractDomainService<User, string, UserRepository>
    {
        /// <summary>
        /// Constructs a User service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public UserService(UnitOfWork unitOfWork, UserRepository repository, ILoggerFactory loggerFactory) 
            : base(unitOfWork, repository, loggerFactory)
        {
        }
    }
}
