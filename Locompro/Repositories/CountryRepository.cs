using Locompro.Data;
using Locompro.Models;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories;

/// <summary>
/// Repository for Country entities.
/// </summary>
public class CountryRepository : AbstractRepository<Country, string>
{
    /// <summary>
    /// Constructs a Country repository for a given context.
    /// </summary>
    /// <param name="context">Context to base the repository on.</param>
    /// <param name="loggerFactory">Factory for repository logger.</param>
    public CountryRepository(LocomproContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory)
    {
    }
}