using Locompro.Models.Entities;
using Locompro.Models.Results;

namespace Locompro.Services.Domain;

/// <summary>
///     Defines the contract for services handling user-related operations.
/// </summary>
public interface IUserService : IDomainService<User, string>
{
    /// <summary>
    ///     Retrieves a list of qualified user IDs for users who are qualified to be moderators.
    /// </summary>
    /// <returns>A list of <see cref="GetQualifiedUserIDsResult" /> objects, each representing a qualified user ID.</returns>
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
    ///    Gets a list with the information of the top 10 users that have been reported order by reported submission count
    /// </summary>
    /// <returns> a list of the information </returns>
    List<MostReportedUsersResult> GetMostReportedUsersInfo();
    
    /// <summary>
    /// Gets the shopping list associated to an user
    /// </summary>
    /// <param name="userId"> The ID of the user who is owner of the shopping list </param>
    /// <returns> the shopping list </returns>
    Task<ShoppingListDto> GetShoppingList(string userId);
    
    /// <summary>
    /// Adds a product to the shopping list of an user
    /// </summary>
    /// <param name="userId"> The ID of the user who is owner of the shopping list </param>
    /// <param name="productId"> ID of the product to be added </param>
    /// <returns></returns>
    Task AddProductToShoppingList(string userId, int productId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"> The ID of the user who is owner of the shopping list </param>
    /// <param name="productId"> The ID of the product to be removed </param>
    /// <returns> ID of the product to be removed </returns>
    Task DeleteProductFromShoppingList(string userId, int productId);
}