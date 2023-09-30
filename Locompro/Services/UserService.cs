using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services;

public class UserService : AbstractDomainService<User, string, UserRepository>
{
    public UserService(UnitOfWork unitOfWork, UserRepository repository) : base(unitOfWork, repository)
    {
    }
}