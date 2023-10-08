using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels
{
    /// <summary>
    /// Incoming data for a user login operation.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the username entered by the user.
        /// </summary>
        /// <value>
        /// The username entered by the user.
        /// </value>
        [Required(ErrorMessage = "Se requiere el nombre de usuario")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        /// <value>
        /// The password entered by the user.
        /// </value>
        [Required(ErrorMessage = "Por favor ingrese una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to be remembered on this device.
        /// </summary>
        /// <value>
        /// True if the user wants to be remembered; otherwise, false. Default value is false.
        /// </value>
        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

}
