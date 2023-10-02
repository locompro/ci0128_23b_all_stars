using Locompro.Data;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    /// <summary>
    /// Generic case for application repositories.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity handled by repository.</typeparam>
    /// <typeparam name="TKey">Type of key used by entity.</typeparam>
    public abstract class AbstractRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly LocomproContext Context;
        protected readonly DbSet<TEntity> DbSet;

        protected AbstractRepository(LocomproContext context)
        {
            this.Context = context;
            DbSet = this.Context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
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
