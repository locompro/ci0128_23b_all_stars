using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locompro.Repositories
{
    public interface IRepository<T, I>
    {
        Task<T> GetByIdAsync(I id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(I id);
    }
}
