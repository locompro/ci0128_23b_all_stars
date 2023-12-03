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

    /// <summary>
    ///     Gets the total number of submissions made by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The total count of submissions made by the user.</returns>
    int GetSubmissionsCountByUser(string userId);

    /// <summary>
    ///     Gets the total number of reported submissions made by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The total count of reported submissions made by the user.</returns>
    int GetReportedSubmissionsCountByUser(string userId);

    /// <summary>
    ///     Gets the total number of rated submissions made by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The total count of rated submissions made by the user.</returns>
    int GetRatedSubmissionsCountByUser(string userId);


    /// <summary>
    ///     Retrieves a list of users with the most reported submissions. Each entry includes the username,
    ///     the count of their reported submissions, and their total submission count.
    /// </summary>
    /// <returns></returns>
    List<MostReportedUsersResult> GetMostReportedUsersInfo();
    
    Task AddProductToShoppingList(string userId, int productId);
    
    Task DeleteProductFromShoppingList(string userId, int productId);
}