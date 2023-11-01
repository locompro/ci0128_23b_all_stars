namespace Locompro.Models.ViewModels;

/// <summary>
///     Represents the profile information of a user.
/// </summary>
public class ProfileViewModel
{
    public string Username { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public float Rating { get; set; }
    
    public int Contributions { get; set; }
    
    public string Email { get; set; }
}