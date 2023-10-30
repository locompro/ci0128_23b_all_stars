using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

public class CantonService : DomainService<Canton, string>, ICantonService
{
    private readonly ICantonRepository _cantonRepository;

    public CantonService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _cantonRepository = UnitOfWork.GetRepository<ICantonRepository>();
    }

    public async Task<Canton> Get(string country, string province, string canton)
    {
        return await _cantonRepository.GetByIdAsync(country, province, canton);
    }
}