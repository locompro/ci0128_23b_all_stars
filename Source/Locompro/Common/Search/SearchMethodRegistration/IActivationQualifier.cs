namespace Locompro.Common.Search.SearchMethodRegistration;

/// <summary>
///     Interface to be implemented by generic class, generic class then decides type when used
/// </summary>
public interface IActivationQualifier
{
    /// <summary>
    /// Gets the qualifier function Func<Type, bool>
    /// </summary>
    /// <returns> Qualifier function </returns>
    dynamic GetQualifierFunction();
}