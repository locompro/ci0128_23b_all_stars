using Locompro.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

public class CantonRepository : CrudRepository<Canton, string>, ICantonRepository
{
    public CantonRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }

    // TODO: Find a more meaningful way to pass composite keys
    public async Task<Canton> GetByIdAsync(string country, string province, string canton)
    {
        return await Set.FindAsync(country, province, canton);
    }
}