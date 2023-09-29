using Locompro.Data;
using Locompro.Models;

namespace Locompro.Repositories;

public class CountryRepository : AbstractRepository<Country, string>
{
    public CountryRepository(LocomproContext context) : base(context)
    {
    }
}