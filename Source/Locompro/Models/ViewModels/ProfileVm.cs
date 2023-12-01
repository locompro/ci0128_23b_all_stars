using Locompro.Models.Entities;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Represents the profile information of a user.
/// </summary>
public class ProfileVm
{
    /// <summary>
    ///     Constructor of ProfileViewModel based on a User object.
    /// </summary>
    /// <param name="user">The user whose information will be displayed.</param>
    public ProfileVm(User user)
    {
        Username = user.UserName;
        Name = user.Name ?? "N/A";
        Address = user.Address ?? "No fue proveído";
        Rating = user.Rating;
        ContributionsCount = user.Submissions != null ? user.CreatedSubmissions.Count : 0;
        Email = user.Email;
    }

    public ProfileVm()
    {
    }

    public string Username { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public float Rating { get; set; }

    public int ContributionsCount { get; set; }

    public string Email { get; set; }
}