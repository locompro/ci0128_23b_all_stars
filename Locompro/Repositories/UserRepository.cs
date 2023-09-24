using Locompro.Models;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories
{
    public class UserRepository : AbstractRepository<User, string>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }


    }
}
