using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories
{
    /// <summary>
    /// Generic repository for an application entity type.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by repository.</typeparam>
    /// <typeparam name="TK">Type of key used by entity.</typeparam>
    public class CrudRepository<T, TK> : ICrudRepository<T, TK> where T: class
    {
        protected readonly ILogger Logger;
        protected readonly DbContext Context;
        protected readonly DbSet<T> Set;

        /// <summary>
        /// Constructs a repository for a given context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory">Factory for repository logger.</param>
        public CrudRepository(DbContext context, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Context = context;
            Set = Context.Set<T>();
        }

        /// <inheritdoc />
        public virtual async Task<T> GetByIdAsync(TK id) => await Set.FindAsync(id);

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync() => await Set.ToListAsync();

        /// <inheritdoc />
        public async Task AddAsync(T entity) {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Set.AddAsync(entity);
        }

        /// <inheritdoc />
        public void UpdateAsync(T entity) {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Set.Update(entity);
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(TK id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                Set.Remove(entity);
            }
        }
    }
}
