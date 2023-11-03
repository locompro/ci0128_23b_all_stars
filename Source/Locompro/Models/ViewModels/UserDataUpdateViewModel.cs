using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Data needed to modify the user's data.
/// </summary>
public class UserDataUpdateViewModel
{
    /// <summary>
    ///     New email address of the user.
    /// </summary>
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [DisplayName("Email")]
    public string Email { get; set; } = "";

    /// <summary>
    ///     The new province for the modified address.
    /// </summary>
    [DisplayName("Provincia")]
    public string Province { get; set; } = "";

    /// <summary>
    ///     New canton for the modified address.
    /// </summary>
    [DisplayName("Cantón")]
    public string Canton { get; set; } = "";

    /// <summary>
    ///     The description of the new address.
    /// </summary>
    [DisplayName("Dirección Exacta")]
    [StringLength(60, ErrorMessage = "La {0} debe poseer entre {2} y {1} caracteres.", MinimumLength = 1)]
    public string ExactAddress { get; set; } = "";

    /// <summary>
    ///     Determines whether the view model holds a valid update for the user's data.
    /// </summary>
    /// <remarks>
    ///    A valid update is one where the user's email or address (formed by Province, Canton, and ExactAddress) is not empty.
    /// </remarks>
    /// <returns>True if is a valid update, false otherwise.</returns>
    public bool IsUpdateValid()
    {
        return !IsEmailEmpty() || !IsAddressEmpty();
    }

    /// <summary>
    ///     Constructs a string representing the user's address by concatenating Province, Canton, and ExactAddress
    ///     properties.
    /// </summary>
    /// <returns>The user's address as a string.</returns>
    public string GetAddress()
    {
        return $"{Province}, {Canton}, {ExactAddress}";
    }

    /// <summary>
    ///     Determines whether the address-related properties in the view model are empty.
    /// </summary>
    /// <returns>True if at least one of the address-related properties are empty, false otherwise.</returns>
    public bool IsAddressEmpty()
    {
        return string.IsNullOrEmpty(Province) || string.IsNullOrEmpty(Canton) || string.IsNullOrEmpty(ExactAddress);
    }

    /// <summary>
    ///     Determines whether the Email property in the view model is empty.
    /// </summary>
    /// <returns>True if the Email property is empty, false otherwise.</returns>
    public bool IsEmailEmpty()
    {
        return string.IsNullOrEmpty(Email);
    }
}