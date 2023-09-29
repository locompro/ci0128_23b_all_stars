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
        protected readonly LocomproContext Context;
        protected readonly DbSet<T> DbSet;

        protected AbstractRepository(LocomproContext context)
        {
            this.Context = context;
            DbSet = this.Context.Set<T>();
        }

        public async Task<T> GetByIdAsync(I id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(I id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}
