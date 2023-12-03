using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Locompro.Models.Results;

namespace Locompro.Services.Domain;

public class UserService : DomainService<User, string>, IUserService
{
    /// <summary>
    ///     The repository for performing user-related operations.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="loggerFactory">The factory used to create loggers.</param>
    public UserService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _userRepository = UnitOfWork.GetSpecialRepository<IUserRepository>();
    }

    /// <inheritdoc/>
    public List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        return _userRepository.GetQualifiedUserIDs();
    }
    
    /// <inheritdoc />
    public int GetSubmissionsCountByUser(string userId)
    {
        return _userRepository.GetSubmissionsCountByUser(userId);
    }
    
    /// <inheritdoc />
    public int GetReportedSubmissionsCountByUser(string userId)
    {
        return _userRepository.GetReportedSubmissionsCountByUser(userId);
    }
    
    /// <inheritdoc />
    public int GetRatedSubmissionsCountByUser(string userId)
    {
        return _userRepository.GetRatedSubmissionsCountByUser(userId);
    }
    
    /// <inheritdoc />
    public List<MostReportedUsersResult> GetMostReportedUsersInfo()
    {
        var results = _userRepository.GetMostReportedUsersInfo();
        results = results.OrderByDescending(x => x.ReportedSubmissionCount).Take(10).ToList();
        return results;
    }
}