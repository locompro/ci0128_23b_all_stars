using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services
{
    /// <summary>
    /// Domain service for Country entities.
    /// </summary>
    public class CountryService : AbstractDomainService<Country, string, CountryRepository>
    {
        /// <summary>
        /// Constructs a Country service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public CountryService(UnitOfWork unitOfWork, CountryRepository repository, ILoggerFactory loggerFactory) 
            : base(unitOfWork, repository, loggerFactory)
        {
        }
    }
}