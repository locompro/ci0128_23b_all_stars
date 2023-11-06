namespace Locompro.Common.Search.SearchMethodRegistration;

/// <summary>
///     A search parameter or way to search for a submission
///     To add a new search parameter, add a new enum to the SearchParameterTypes enum and add a new entry to the
///     SearchParameters dictionary
/// </summary>
public class SearchParam : ISearchParam
{
    public ISearchQuery SearchQuery { get; set; }
    public IActivationQualifier ActivationQualifier { get; set; }

    /// <summary>
    ///     Returns the activation qualifier of the type of the generic implementation
    /// </summary>
    /// <returns></returns>
    public IActivationQualifier GetActivationQualifier()
    {
        return ActivationQualifier;
    }
}