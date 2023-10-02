using Locompro.Data;
using Locompro.Models;

namespace Locompro.Repositories
{
    public class CategoryRepository : AbstractRepository<Category, string>
    {
        public CategoryRepository(LocomproContext context) : base(context)
        {
        }
    }
}