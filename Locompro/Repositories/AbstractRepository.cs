using Locompro.Data;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    /// <summary>
    /// Generic repository for an application entity type.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by repository.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    public abstract class AbstractRepository<T, I> : IRepository<T, I> where T : class
    {
        protected readonly ILogger Logger;
        protected readonly LocomproContext Context;
        protected readonly DbSet<T> DbSet;

        /// <summary>
        /// Constructs a repository for a given context.
        /// </summary>
        /// <param name="context">Context to base the repository on.</param>
        /// <param name="loggerFactory">Factory for repository logger.</param>
        protected AbstractRepository(LocomproContext context, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Context = context;
            DbSet = Context.Set<T>();
        }

        /// <inheritdoc />
        public async Task<T> GetByIdAsync(I id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <inheritdoc />
        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc />
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
