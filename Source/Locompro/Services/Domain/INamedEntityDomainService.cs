namespace Locompro.Services.Domain;

public interface INamedEntityDomainService<T, I> : IDomainService<T, I> where T : class 
{
    Task<IEnumerable<T>> GetByPartialName(string partialName);
}