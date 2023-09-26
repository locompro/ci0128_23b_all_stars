using Locompro.Models;

namespace Locompro.Repositories
{
    /// <summary>
    /// Interface representing application repositories.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by repository.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    public interface IRepository<T, I>
    {
        Task<T> GetByIdAsync(I id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(I id);
    }
}
