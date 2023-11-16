namespace Locompro.Common.Search.SearchMethodRegistration;

/// <summary>
///     Generic class for condition for search query to be considered in search process
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActivationQualifier<T> : IActivationQualifier
{
    private Func<T, bool> QualifierFunction { get; init; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="qualifierFunction"> function that determines if another is executed or not </param>
    public ActivationQualifier(Func<T, bool> qualifierFunction)
    {
        QualifierFunction = qualifierFunction;
    }
    
    /// <inheritdoc />
    public dynamic GetQualifierFunction()
    {
        return QualifierFunction;
    }
}