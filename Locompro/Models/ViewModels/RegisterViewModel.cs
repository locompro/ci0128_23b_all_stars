using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels
{
    /// <summary>
    /// Represents the data required for a user registration operation.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the email address entered by the user.
        /// </summary>
        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username entered by the user.
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(100, ErrorMessage = "La {0} debe poseer entre {2} y {1} caracteres.", MinimumLength = 6)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{6,}$",
            ErrorMessage =
                "La contraseña debe poseer al menos una letra mayúscula, una minúscula, un número y un caracter especial. Minimo 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation entered by the user.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

}
