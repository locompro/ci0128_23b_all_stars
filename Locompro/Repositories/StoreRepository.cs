using Locompro.Data;
using Locompro.Models;

namespace Locompro.Repositories
{
    public class StoreRepository : AbstractRepository<Store, string>
    {
        public StoreRepository(LocomproContext context) : base(context)
        {
        }
    }
}
