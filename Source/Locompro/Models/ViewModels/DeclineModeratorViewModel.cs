using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

public class DeclineModeratorViewModel
{
    /// <summary>
    ///     The current password of the user.
    /// </summary>
    [DisplayName("Contraseña actual")]
    public string CurrentPassword { get; set; }
}