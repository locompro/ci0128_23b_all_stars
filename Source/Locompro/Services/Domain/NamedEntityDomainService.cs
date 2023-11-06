using Locompro.Data;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

/// <summary>
/// Generic domain service for an application entity type with string ID.
/// </summary>
/// <typeparam name="T">Type of entity handled by service.</typeparam>
/// <typeparam name="TK"></typeparam>
public class NamedEntityDomainService<T, TK> : DomainService<T, TK>, INamedEntityDomainService<T, TK> where T : class
{
    private readonly INamedEntityRepository<T, TK> _namedEntityRepository;

    public NamedEntityDomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork,
        loggerFactory)
    {
        _namedEntityRepository = UnitOfWork.GetNamedEntityRepository<T, TK>();
    }

    public async Task<IEnumerable<T>> GetByPartialName(string partialName)
    {
        return await _namedEntityRepository.GetByPartialNameAsync(partialName);
    }
}