using Locompro.Models;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    public class StoreRepository : AbstractRepository<Store, string>
    {
        public StoreRepository(DbContext context) : base(context)
        {
        }
    }
}
