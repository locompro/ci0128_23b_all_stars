namespace Locompro.Data.Repositories;

public interface INamedEntityRepository<T, TK> : ICrudRepository<T, TK> where T : class
{
    Task<IEnumerable<T>> GetByPartialNameAsync(string partialName);
}