using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Locompro.Models.Results;

namespace Locompro.Services.Domain;

public class UserService : Service, IUserService
{
    /// <summary>
    ///     The repository for performing user-related operations.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for coordinating the work of multiple repositories.</param>
    /// <param name="loggerFactory">The factory used to create loggers.</param>
    public UserService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        // Retrieves the user repository from the unit of work
        _userRepository = UnitOfWork.GetSpecialRepository<IUserRepository>();
    }

    /// <inheritdoc/>
    public List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        // Delegates the call to the user repository
        return _userRepository.GetQualifiedUserIDs();
    }
}