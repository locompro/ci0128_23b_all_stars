using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

namespace Locompro.Common.Search;

public interface ISearchQueryParameters<TSearchResult>
{
    ISearchQueryParameters<TSearchResult> AddQueryParameter<TSearchParameter>(SearchParameterTypes searchParameterType, TSearchParameter parameter);
    
    ISearchQueryParameters<TSearchResult> AddFilterParameter<TSearchParameter>(SearchParameterTypes searchParameterType, TSearchParameter parameter);
    
    List<ISearchCriterion> GetQueryParameters();
    
    List<ISearchCriterion> GetSearchFilters();

    void Clear();
}