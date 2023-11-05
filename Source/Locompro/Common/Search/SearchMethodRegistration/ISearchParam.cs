namespace Locompro.Common.Search.SearchMethodRegistration;

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
    
    /// <summary>
    /// Returns internal activation qualifier depending on the type of the generic implementation
    /// </summary>
    /// <returns></returns>
    IActivationQualifier GetActivationQualifier();
}