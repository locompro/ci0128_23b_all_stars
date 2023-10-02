using Locompro.Data;
using Locompro.Models;

namespace Locompro.Repositories
{
    /// <summary>
    /// Repository for Store entities.
    /// </summary>
    public class StoreRepository : AbstractRepository<Store, string>
    {
        /// <summary>
        /// Constructs a Store repository for a given context.
        /// </summary>
        /// <param name="context">Context to base the repository on.</param>
        public StoreRepository(LocomproContext context) : base(context)
        {
        }
    }
}
