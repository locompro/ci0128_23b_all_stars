using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Locompro.Common;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _conditionMethodName;
    private readonly object _conditionValue;

    public RequiredIfAttribute(string conditionMethodName, object conditionValue, string errorMessage)
        : base(errorMessage)
    {
        _conditionMethodName = conditionMethodName;
        _conditionValue = conditionValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();

        // Attempt to find the specified method
        var methodInfo = type.GetMethod(_conditionMethodName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (methodInfo == null) return new ValidationResult("Method not found.");

        // Invoke the method and get the return value
        var returnValue = methodInfo.Invoke(instance, null);

        if (Equals(returnValue, _conditionValue))
            if (value == null)
                return new ValidationResult(ErrorMessageString);

        return ValidationResult.Success;
    }
}