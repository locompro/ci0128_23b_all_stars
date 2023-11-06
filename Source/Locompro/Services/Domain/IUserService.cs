using Locompro.Models;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Defines the contract for services handling user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a list of qualified user IDs for users who are qualified to be moderators.
        /// </summary>
        /// <returns>A list of <see cref="GetQualifiedUserIDsResult"/> objects, each representing a qualified user ID.</returns>
        List<GetQualifiedUserIDsResult> GetQualifiedUserIDs();
    }
}