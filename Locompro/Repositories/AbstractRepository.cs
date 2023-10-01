using Locompro.Data;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    /// <summary>
    /// Generic case for application repositories.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by repository.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    public abstract class AbstractRepository<T, I> : IRepository<T, I> where T : class
    {
        protected readonly LocomproContext context;
        protected readonly DbSet<T> dbSet;

        protected AbstractRepository(LocomproContext context)
        {
            this.context = context;
            dbSet = this.context.Set<T>();
        }

        public async Task<T> GetByIdAsync(I id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(I id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
