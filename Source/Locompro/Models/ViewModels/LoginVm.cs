using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;
/// <summary>
///     Login information of a user.
/// </summary>
public class LoginVm
{

    [Required(ErrorMessage = "Se requiere el nombre de usuario")]
    [Display(Name = "Nombre de usuario")]
    public string UserName { get; set; }


    [Required(ErrorMessage = "Por favor ingrese una contraseña")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Recordarme")] public bool RememberMe { get; set; }
};
