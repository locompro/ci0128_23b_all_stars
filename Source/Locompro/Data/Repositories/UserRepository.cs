using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Models.Results;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

/// <summary>
/// A repository for performing CRUD operations on users within the Locompro context.
/// </summary>
public class UserRepository : CrudRepository<User, string>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to be used by this repository.</param>
    /// <param name="loggerFactory">The factory used to create loggers for logging.</param>
    public UserRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }

    /// <summary>
    /// Retrieves a list of qualified user IDs asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="GetQualifiedUserIDsResult"/> representing qualified users.</returns>
    public List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        // Cast the base Context to LocomproContext to access specific stored procedures or functions.
        var myContext = (LocomproContext)Context;
        // Query to execute the stored procedure or function that gets qualified user IDs.
        var result = from p in myContext.GetQualifiedUserIDs()
            select p;
        return result.ToList();
    }
}