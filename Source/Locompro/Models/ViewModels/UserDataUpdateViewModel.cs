using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Castle.Core.Internal;
using Locompro.Models.ViewModels.Validation;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Data needed to modify the user's data.
/// </summary>
[AddressValidation]
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
    public string ExactAddress { get; set; } = "";

    /// <summary>
    ///     Determines whether all the properties in the view model are empty.
    /// </summary>
    /// <returns>True if all properties are empty, false otherwise.</returns>
    public bool IsEmpty()
    {
        return Email.IsNullOrEmpty() && Province.IsNullOrEmpty() && Canton.IsNullOrEmpty() &&
               ExactAddress.IsNullOrEmpty();
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
    /// <returns>True if the address-related properties are empty, false otherwise.</returns>
    public bool IsAddressEmpty()
    {
        return Province.IsNullOrEmpty() && Canton.IsNullOrEmpty() && ExactAddress.IsNullOrEmpty();
    }

    /// <summary>
    ///     Determines whether the Email property in the view model is empty.
    /// </summary>
    /// <returns>True if the Email property is empty, false otherwise.</returns>
    public bool IsEmailEmpty()
    {
        return Email.IsNullOrEmpty();
    }
}