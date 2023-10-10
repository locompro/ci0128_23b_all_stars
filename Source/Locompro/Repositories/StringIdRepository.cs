using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories;

/// <summary>
/// Generic repository for an application entity type with string ID.
/// </summary>
/// <typeparam name="T">Type of entity handled by repository.</typeparam>
public class StringIdRepository<T> : AbstractRepository<T, string> where T: class
{
    /// <summary>
    /// Constructs a string ID repository for a given context.
    /// </summary>
    /// <param name="context">Context to base the repository on.</param>
    /// <param name="loggerFactory">Factory for repository logger.</param>
    protected StringIdRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
    
    public async Task<IEnumerable<T>> GetByPartialIdAsync(string partialId)
    {
        // Get the primary key property name for entity T
        var keyName = Context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.Select(x => x.Name).FirstOrDefault();

        if (string.IsNullOrEmpty(keyName))
        {
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a defined primary key.");
        }

        return await DbSet.Where(e => EF.Property<string>(e, keyName).Contains(partialId)).ToListAsync();
    }
}
