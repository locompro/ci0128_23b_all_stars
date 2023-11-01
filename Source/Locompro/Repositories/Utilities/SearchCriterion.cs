using Locompro.Repositories.Utilities.Interfaces;

namespace Locompro.Repositories.Utilities;

/// <summary>
/// A search criterion is a search parameter and a search value
/// Is used on the QueryBuilder class to add a new search parameter to be searched
/// </summary>
public class SearchCriterion<T> : ISearchCriterion
{
    public SearchParameterTypes ParameterName { get; init; }
    public T SearchValue { get; init; }
    
    /// <summary>
    /// Default constructor
    /// </summary>
    public SearchCriterion()
    {
    }
    
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