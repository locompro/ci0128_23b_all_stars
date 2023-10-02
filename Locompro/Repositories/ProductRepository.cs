using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Repositories
{
    public class ProductRepository : AbstractRepository<Product, string>
    {
        public ProductRepository(LocomproContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
    }
}
