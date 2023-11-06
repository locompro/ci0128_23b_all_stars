using Locompro.Models.Entities;
using Locompro.Models.Results;

namespace Locompro.Data.Repositories;

/// <summary>
///     Repository for accesing specific user related database operations, such as stored procedures
/// </summary>
public interface IUserRepository : ICrudRepository<User, string>
{
    /// <summary>
    ///     Gets a list users that are qualified to be moderators
    /// </summary>
    List<GetQualifiedUserIDsResult> GetQualifiedUserIDs();
}