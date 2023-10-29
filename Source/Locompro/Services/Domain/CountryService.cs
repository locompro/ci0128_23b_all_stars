using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Domain service for Country entities.
    /// </summary>
    public class CountryService : DomainService<Country, string>
    {
        /// <summary>
        /// Constructs a Country service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public CountryService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
        {
        }
    }
}