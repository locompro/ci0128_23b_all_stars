using System.Security.Claims;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.Auth;

/// <summary>
///     Provides an interface for managing user data within the application.
/// </summary>
public interface IUserManagerService
{
    /// <summary>
    ///     Asynchronously creates a new user with the specified password.
    /// </summary>
    /// <param name="user">The user to be created.</param>
    /// <param name="password">The password for the user.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="IdentityResult" />.</returns>
    Task<IdentityResult> CreateAsync(User user, string password);

    /// <summary>
    ///     Asynchronously retrieves a user based on the provided claims principal.
    /// </summary>
    /// <param name="principal">The claims principal which identifies the user.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="User" />.</returns>
    Task<User> GetUserAsync(ClaimsPrincipal principal);

    /// <summary>
    ///     Asynchronously changes the password for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to change the password.</param>
    /// <param name="currentPassword">The current password of the user.</param>
    /// <param name="newPassword">The new password for the user.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="IdentityResult" />.</returns>
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

    /// <summary>
    ///     Gets a value indicating whether the user manager supports user email.
    /// </summary>
    bool SupportsUserEmail { get; }

    /// <summary>
    ///     Asynchronously checks the password for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to check the password.</param>
    /// <param name="password">The password to check.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="bool" /> indicating
    ///     whether the password is correct.
    /// </returns>
    Task<bool> CheckPasswordAsync(User user, string password);

    /// <summary>
    /// Asynchronously finds a user based on their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier for the user.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation with a result of <see cref="User"/>.</returns>
    Task<User> FindByIdAsync(string userId);

    /// <summary>
    /// Asynchronously determines whether the specified user is in the given role.
    /// </summary>
    /// <param name="user">The user to check.</param>
    /// <param name="role">The role to check for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation with a result of <see cref="bool"/> indicating whether the user is in the specified role.</returns>
    Task<bool> IsInRoleAsync(User user, string role);

    /// <summary>
    /// Asynchronously adds a claim to a user.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claim">The claim to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation of adding a claim to the user.
    /// The task result contains an <see cref="IdentityResult"/> indicating the success
    /// or failure of the operation.
    /// </returns>
    public Task<IdentityResult> AddClaimAsync(User user, Claim claim);

    /// <summary>
    /// Asynchronously deletes a claim from a user.
    /// </summary>
    /// <param name="user">The user from whom the claim is to be deleted.</param>
    /// <param name="claim">The claim to be deleted from the user.</param>
    /// <returns>
    /// A task that represents the asynchronous delete operation. The task result contains
    /// the <see cref="IdentityResult"/> of the delete operation.
    /// </returns>
    public Task<IdentityResult> DeleteClaimAsync(User user, Claim claim);

    /// <summary>
    /// Asynchronously retrieves claims of the specified types for the given user.
    /// </summary>
    /// <param name="user">The user for whom to retrieve claims.</param>
    /// <param name="claimTypes">An enumerable of claim type strings to specify which claims to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous read operation. The task result contains
    /// an enumerable of <see cref="Claim"/> objects that match the specified claim types.
    /// </returns>
    public Task<IList<Claim>> GetClaimsOfTypesAsync(User user, string claimType);
}