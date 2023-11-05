using Locompro.Models.Entities;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Represents the profile information of a user.
/// </summary>
public class ProfileVm
{
    public string Username { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public float Rating { get; set; }
    
    public int Contributions { get; set; }
    
    public string Email { get; set; }
    
    /// <summary>
    ///     Constructor of ProfileViewModel based on a User object.
    /// </summary>
    /// <param name="user">The user whose information will be displayed.</param>
    public ProfileVm(User user)
    {
        Username = user.UserName;
        Name = user.Name;
        Address = user.Address;
        Rating = user.Rating;
        Contributions = user.Submissions.Count;
        Email = user.Email;
    }
    public ProfileVm()
    {
    }
}