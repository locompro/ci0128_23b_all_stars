namespace Locompro.Data.Repositories;

public interface INamedEntityRepository<T, I> : ICrudRepository<T, I> where T : class
{
    Task<IEnumerable<T>> GetByPartialNameAsync(string partialName);
}