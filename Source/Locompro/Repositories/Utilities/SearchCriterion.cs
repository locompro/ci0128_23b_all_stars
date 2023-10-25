namespace Locompro.Repositories.Utilities;

/// <summary>
/// A search criterion is a search parameter and a search value
/// Is used on the QueryBuilder class to add a new search parameter to be searched
/// </summary>
public class SearchCriterion
{
    public SearchParam.SearchParameterTypes ParameterName { get; init; }
    public string SearchValue { get; init; }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="searchValue"></param>
    public SearchCriterion(SearchParam.SearchParameterTypes parameterName = default, string searchValue = "")
    {
        this.ParameterName = parameterName;
        this.SearchValue = searchValue;
    }
}