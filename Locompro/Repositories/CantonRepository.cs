using Locompro.Models;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    public class CantonRepository : AbstractRepository<Canton, string>
    {
        public CantonRepository(LocomproContext context) : base(context)
        {
        }

        // get all cantons for a given province
        public async Task<IEnumerable<Canton>> GetCantons(String provinceName)
        {
            return await this.DbSet // from set
                 .Select(c => c) // select all cantons
                    .Where(c=> c.Province.Name == provinceName) // located in the given province
                        .ToListAsync(); // and place on list asynchronously
        }
    }
}
