using Locompro.Data;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

/// <summary>
/// Generic domain service for an application entity type with string ID.
/// </summary>
/// <typeparam name="T">Type of entity handled by service.</typeparam>
/// <typeparam name="I"></typeparam>
public class NamedEntityDomainService<T, I> : DomainService<T, I>, INamedEntityDomainService<T, I> where T : class
{
    private readonly INamedEntityRepository<T, I> _namedEntityRepository;

    public NamedEntityDomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork,
        loggerFactory)
    {
        _namedEntityRepository = UnitOfWork.GetRepository<INamedEntityRepository<T, I>>();
    }

    public async Task<IEnumerable<T>> GetByPartialName(string partialName)
    {
        return await _namedEntityRepository.GetByPartialNameAsync(partialName);
    }
}