using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Represents the data required to change a user's password.
/// </summary>
public class PasswordChangeVm
{
    /// <summary>
    ///     The current password of the user.
    /// </summary>
    [Required(ErrorMessage = "La contraseña actual es requerida.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{6,}$",
        ErrorMessage =
            "La contraseña actual debe tener al menos una letra mayúscula, una minúscula, un número y un caracter especial. Minimo 6 caracteres.")]
    [DataType(DataType.Password)]
    [DisplayName("Contraseña actual")]
    public string CurrentPassword { get; set; }

    /// <summary>
    ///     The new password for the user.
    /// </summary>
    [Required(ErrorMessage = "La nueva contraseña es requerida.")]
    [StringLength(100, ErrorMessage = "La {0} debe poseer entre {2} y {1} caracteres.", MinimumLength = 6)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{6,}$",
        ErrorMessage =
            "La contraseña nueva debe poseer al menos una letra mayúscula, una minúscula, un número y un caracter especial. Minimo 6 caracteres.")]
    [DataType(DataType.Password)]
    [DisplayName("Contraseña")]
    public string NewPassword { get; set; }

    /// <summary>
    ///     Confirmation of the new password for the user.
    /// </summary>
    [DataType(DataType.Password)]
    [DisplayName("Confirmar contraseña")]
    [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmNewPassword { get; set; }
}