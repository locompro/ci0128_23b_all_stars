using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services
{
    /// <summary>
    /// Service for Country entities.
    /// </summary>
    public class CountryService : AbstractDomainService<Country, string, CountryRepository>
    {
        public CountryService(UnitOfWork unitOfWork, CountryRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}