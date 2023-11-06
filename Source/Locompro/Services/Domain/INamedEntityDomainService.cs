namespace Locompro.Services.Domain;

public interface INamedEntityDomainService<T, TK> : IDomainService<T, TK> where T : class
{
    Task<IEnumerable<T>> GetByPartialName(string partialName);
}