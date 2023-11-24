using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

namespace Locompro.Common.Search.SearchQueryParameters;

public class SearchQueryParameters<TSearchResult> : ISearchQueryParameters<TSearchResult>
{
    private readonly List<ISearchCriterion> _queryParameters;
    private readonly List<ISearchCriterion> _searchFilters;

    public SearchQueryParameters()
    {
        _queryParameters = new List<ISearchCriterion>();
        _searchFilters = new List<ISearchCriterion>();
    }

    public ISearchQueryParameters<TSearchResult> AddQueryParameter<TSearchParameter>(SearchParameterTypes searchParameterType, TSearchParameter parameter)
    {
        _queryParameters.Add(new SearchCriterion<TSearchParameter>(searchParameterType, parameter));
        return this;
    }

    public ISearchQueryParameters<TSearchResult> AddFilterParameter<TSearchParameter>(SearchParameterTypes searchParameterType,
        TSearchParameter parameter)
    {
        _searchFilters.Add(new SearchCriterion<TSearchParameter>(searchParameterType, parameter));

        return this;
    }
    
    public List<ISearchCriterion> GetQueryParameters()
    {
        return _queryParameters;
    }

    public List<ISearchCriterion> GetSearchFilters()
    {
        return _searchFilters;
    }

    public void Clear()
    {
        _queryParameters.Clear();
        _searchFilters.Clear();
    }
}