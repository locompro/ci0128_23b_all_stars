namespace Locompro.Common.Search.QueryBuilder;

public interface IQueryBuilder<TSearchResults>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchQueryParameters"></param>
    void AddSearchCriteria(ISearchQueryParameters<TSearchResults> searchQueryParameters);
    
    /// <summary>
    ///     Returns the list of search functions that can be used to filter the results of a query
    /// </summary>
    /// <returns></returns>
    public ISearchQueries<TSearchResults> GetSearchFunction();

    /// <summary>
    ///     Adds a search criterion to the list of search criteria
    ///     A search criterion is a search parameter and a search value
    /// </summary>
    /// <param name="searchCriterion"></param>
    void AddSearchCriterion(ISearchCriterion searchCriterion);
    
    /// <summary>
    ///     Resets the builder class
    /// </summary>
    public void Reset();
}