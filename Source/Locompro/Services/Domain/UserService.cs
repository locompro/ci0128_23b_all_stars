using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

/// <summary>
/// Domain service for User entities.
/// </summary>
public class UserService : DomainService<User, string>
{
    /// <summary>
    /// Constructs a User service for a given repository.
    /// </summary>
    /// <param name="unitOfWork">Unit of work to handle transactions.</param>
    /// <param name="loggerFactory">Factory for service logger.</param>
    public UserService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
    }
}