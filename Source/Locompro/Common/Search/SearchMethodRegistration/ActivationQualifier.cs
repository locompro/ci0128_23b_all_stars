namespace Locompro.Common.Search.SearchMethodRegistration;

/// <summary>
/// Generic class for condition for search query to be considered in search process
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActivationQualifier<T> : IActivationQualifier
{
    public Func<T, bool> QualifierFunction { get; init; }
    
    /// <summary>
    /// returns the internal qualifier
    /// </summary>
    /// <returns></returns>
    public dynamic GetQualifierFunction()
    {
        return QualifierFunction;
    }
}