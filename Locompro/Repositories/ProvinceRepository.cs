using Locompro.Models;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    public class ProvinceRepository : AbstractRepository<Province, string>
    {
        public ProvinceRepository(LocomproContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Province>> GetAllProvinces()
        {
            IQueryable<Province> querable = this.DbSet.Select(p => p);

            return await querable.ToListAsync();
        }
    }
}
