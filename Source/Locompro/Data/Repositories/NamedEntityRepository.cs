using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Locompro.Data.Repositories;

/// <summary>
/// Generic repository for an application entity type with string ID.
/// </summary>
/// <typeparam name="T">Type of entity handled by repository.</typeparam>
public class NamedEntityRepository<T, TK> : CrudRepository<T, TK>, INamedEntityRepository<T, TK> where T: class
{
    /// <summary>
    /// Constructs a string ID repository for a given context.
    /// </summary>
    /// <param name="context">Context to base the repository on.</param>
    /// <param name="loggerFactory">Factory for repository logger.</param>
    public NamedEntityRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
    
    public async Task<IEnumerable<T>> GetByPartialNameAsync(string partialName)
    {
        return await Set.Where($"Name.Contains(@0)", partialName).ToListAsync();
    }
}
