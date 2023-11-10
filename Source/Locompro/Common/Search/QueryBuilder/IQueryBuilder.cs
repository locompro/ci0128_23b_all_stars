namespace Locompro.Common.Search.QueryBuilder;

public interface IQueryBuilder
{
    /// <summary>
    ///     Adds a search criterion to the list of search criteria
    ///     A search criterion is a search parameter and a search value
    /// </summary>
    /// <param name="searchCriterion"></param>
    public void AddSearchCriterion(ISearchCriterion searchCriterion);
    
    /// <summary>
    ///     Returns the list of search functions that can be used to filter the results of a query
    /// </summary>
    /// <returns></returns>
    public ISearchQueries GetSearchFunction();
    
    /// <summary>
    ///     Resets the builder class
    /// </summary>
    public void Reset();
}