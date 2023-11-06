using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Common.Search;

/// <summary>
///     A search criterion is a search parameter and a search value
///     Is used on the QueryBuilder class to add a new search parameter to be searched
/// </summary>
public class SearchCriterion<T> : ISearchCriterion
{
    /// <summary>
    ///     Default constructor
    /// </summary>
    public SearchCriterion()
    {
    }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="searchValue"></param>
    public SearchCriterion(SearchParameterTypes parameterName, T searchValue)
    {
        ParameterName = parameterName;
        SearchValue = searchValue;
    }

    public T SearchValue { get; init; }
    public SearchParameterTypes ParameterName { get; init; }

    /// inheritedDoc
    public dynamic GetSearchValue()
    {
        return SearchValue;
    }
}