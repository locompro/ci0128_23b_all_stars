using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

public class PasswordChageViewModel
{
    [Required(ErrorMessage = "La contraseña es requerida.")]
    public string CurrentPassword { get; set; }
    
    [Required(ErrorMessage = "La nueva contraseña es requerida.")]
    [StringLength(100, ErrorMessage = "La {0} debe poseer entre {2} y {1} caracteres.", MinimumLength = 6)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{6,}$",
        ErrorMessage =
            "La contraseña debe poseer al menos una letra mayúscula, una minúscula, un número y un caracter especial. Minimo 6 caracteres.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string NewPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmNewPassword { get; set; }

}
