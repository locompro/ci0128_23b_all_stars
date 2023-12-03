using Locompro.Models.Entities;
using Locompro.Models.Results;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

/// <summary>
///     A repository for performing CRUD operations on users within the Locompro context.
/// </summary>
public class UserRepository : CrudRepository<User, string>, IUserRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserRepository" /> class.
    /// </summary>
    /// <param name="context">The database context to be used by this repository.</param>
    /// <param name="loggerFactory">The factory used to create loggers for logging.</param>
    public UserRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }

    /// <summary>
    ///     Retrieves a list of qualified user IDs asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="GetQualifiedUserIDsResult" /> representing qualified users.</returns>
    public List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        // Cast the base Context to LocomproContext to access specific stored procedures or functions.
        var myContext = (LocomproContext)Context;
        // Query to execute the stored procedure or function that gets qualified user IDs.
        var result = from p in myContext.GetQualifiedUserIDs()
            select p;
        return result.ToList();
    }

    /// <inheritdoc />
    public int GetSubmissionsCountByUser(string userId)
    {
        try
        {
            var locomproContext = (LocomproContext)Context;
            var result = locomproContext.CountSubmissions(userId);
            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting submissions count by user {userId}", userId);
            throw;
        }
    }

    /// <inheritdoc />
    public int GetReportedSubmissionsCountByUser(string userId)
    {
        try
        {
            var locomproContext = (LocomproContext)Context;
            var result = locomproContext.CountReportedSubmissions(userId);
            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting reported submissions count by user {userId}", userId);
            throw;
        }
    }

    /// <inheritdoc />
    public int GetRatedSubmissionsCountByUser(string userId)
    {
        try
        {
            var locomproContext = (LocomproContext)Context;
            var result = locomproContext.CountRatedSubmissions(userId);
            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting rated submissions count by user {userId}", userId);
            throw;
        }
    }

    /// <inheritdoc />
    public List<MostReportedUsersResult> GetMostReportedUsersInfo()
    {
        try
        {
            var locomproContext = (LocomproContext)Context;
            var results = from result in locomproContext.GetMostReportedUsersResults()
                select result;
            return results.ToList();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting most reported users");
            throw;
        }
    }

    public Task AddProductToShoppingList(string userId, int productId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProductFromShoppingList(string userId, int productId)
    {
        throw new NotImplementedException();
    }
}