using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Common.Search.SearchFilters;

public class SearchFilterParam : ISearchFilterParam
{
    private ISearchFilterQuery SearchQuery { get; set; }
    public IActivationQualifier ActivationQualifier { get; set; }
    
    public SearchFilterParam(
        ISearchFilterQuery searchQuery,
        IActivationQualifier activationQualifier)
    {
        SearchQuery = searchQuery;
        ActivationQualifier = activationQualifier;
    }
    
    public IActivationQualifier GetActivationQualifier()
    {
        return ActivationQualifier;
    }

    public ISearchFilterQuery GetSearchQuery()
    {
        return SearchQuery;
    }
}