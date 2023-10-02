using Locompro.Data;
using Locompro.Models;

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
    public CountryRepository(LocomproContext context) : base(context)
    {
    }
}