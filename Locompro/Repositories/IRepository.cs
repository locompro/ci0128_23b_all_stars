using Locompro.Models;

namespace Locompro.Repositories
{
    /// <summary>
    /// Interface representing application repositories.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity handled by repository.</typeparam>
    /// <typeparam name="TKey">Type of key used by entity.</typeparam>
    public interface IRepository<TEntity, TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
    }
}
