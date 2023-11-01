using Locompro.SearchQueryConstruction;

namespace Locompro.SearchQueryConstruction;

/// <summary>
/// A search criterion is a search parameter and a search value
/// Is used on the QueryBuilder class to add a new search parameter to be searched
/// </summary>
public class SearchCriterion<T> : ISearchCriterion
{
    public SearchParameterTypes ParameterName { get; init; }
    public T SearchValue { get; init; }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="searchValue"></param>
    public SearchCriterion(SearchParameterTypes parameterName, T searchValue)
    {
        ParameterName = parameterName;
        SearchValue = searchValue;
    }

    public dynamic GetSearchValue()
    {
        return SearchValue;
    }
}