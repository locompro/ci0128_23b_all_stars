namespace Locompro.SearchQueryConstruction;

public interface ISearchParam
{
    /// <summary>
    /// Function or expression of how to find a submission
    /// </summary>
    public ISearchQuery SearchQuery { get; set; }

    /// <summary>
    /// Function or expression of whether to perform the search or not
    /// </summary>
    public IActivationQualifier ActivationQualifier { get; set; }

    IActivationQualifier GetActivationQualifier();
}