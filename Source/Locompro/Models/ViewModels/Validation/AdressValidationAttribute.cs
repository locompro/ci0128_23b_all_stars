using System.ComponentModel.DataAnnotations;
using Castle.Core.Internal;

namespace Locompro.Models.ViewModels.Validation;

/// <summary>
///     Attribute to validate the new address data.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AddressValidationAttribute : ValidationAttribute
{
    /// <summary>
    ///     Validates the address. If the province is selected, the canton must be selected too and the exact address must be
    ///     filled.
    /// </summary>
    /// <param name="value">The current value of the object being validated. This parameter is not used in this method.</param>
    /// <param name="validationContext">
    ///     The context information about the validation operation, including the object instance
    ///     being validated.
    /// </param>
    /// <returns>A ValidationResult object that encapsulates the result of the validation and any validation error messages.</returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not UserDataUpdateViewModel model)
            return new ValidationResult("Unexpected model type.");
        if (!model.Province.IsNullOrEmpty() && model.Canton.IsNullOrEmpty())
            return new ValidationResult("Si selecciona una provincia debe seleccionar un cantón.");
        if (!model.Province.IsNullOrEmpty() && !model.Canton.IsNullOrEmpty() && model.ExactAddress.IsNullOrEmpty())
            return new ValidationResult("La dirección exacta es necesaria si selecciono Provincia y Canton.");

        return ValidationResult.Success;
    }

}