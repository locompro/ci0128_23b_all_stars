using Locompro.Models;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories;

public class CantonRepository : AbstractRepository<Canton, string>
{
    public CantonRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
    
    public async Task<Canton> GetByIdAsync(string country, string province, string canton)
    {
        return await DbSet.FindAsync(country, province, canton);
    }
}