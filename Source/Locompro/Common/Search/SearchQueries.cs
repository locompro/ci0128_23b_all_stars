using System.Linq.Expressions;
using Castle.Core.Internal;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Models.Entities;

namespace Locompro.Common.Search;

/// <summary>
///     Class for encapsulating all data related to a search criterion
///     If there were other types of criteria or functions to be used, then add to this class
/// </summary>
public class SearchQueries<TSearchResult> : ISearchQueries<TSearchResult>
{
    private readonly List<Expression<Func<TSearchResult, bool>>> _searchQueryFunctions;
    
    private readonly Func<TSearchResult, bool> _searchQueryFilters;

    private List<Expression<Func<TSearchResult, bool>>> _uniqueSearchExpressions;
    
    private bool _noFilters { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchQueryFunctions"> search query to be stored </param>
    /// <param name="searchQueryFilters"></param>
    /// <param name="uniqueSearchExpressions"></param>
    public SearchQueries(List<Expression<Func<TSearchResult, bool>>> searchQueryFunctions,
        List<Func<TSearchResult, bool>> searchQueryFilters,
        List<Expression<Func<TSearchResult, bool>>> uniqueSearchExpressions)
    {
        _searchQueryFunctions = searchQueryFunctions;

        _noFilters = searchQueryFilters is null || searchQueryFilters.Count == 0;

        if (searchQueryFilters != null)
            _searchQueryFilters = _noFilters
                ? x => true
                : searchQueryFilters.Aggregate((current, next) => x => current(x) && next(x));

        _uniqueSearchExpressions = uniqueSearchExpressions;
    }
    
    /// <inheritdoc />
    public bool IsEmpty() => _searchQueryFunctions.Count == 0;

    public bool NoSearchFilters()
    {
        return _noFilters;
    }

    /// <inheritdoc />
    public int Count()
    {
        return _searchQueryFunctions.Count;
    }
    
    /// <inheritdoc />
    public IQueryable<TSearchResult> ApplySearch(IQueryable<TSearchResult> queryable)
    {
        IQueryable<TSearchResult> results = 
            _searchQueryFunctions.Aggregate(
                queryable ,(current, query) =>current.Where(query));
        
       return results;
    }
    
    public IEnumerable<TSearchResult> ApplySearchFilters(IEnumerable<TSearchResult> unfilteredResults)
    {
        return unfilteredResults.Where(_searchQueryFilters);
    }
    
    public IQueryable<TSearchResult> ApplyUniqueSearches(IQueryable<TSearchResult> queryable)
    {
        if (_uniqueSearchExpressions is null || _uniqueSearchExpressions.Count == 0)
        {
            return queryable;
        }
        
        IQueryable<TSearchResult> results = 
            _uniqueSearchExpressions.Aggregate(
                queryable ,(current, query) =>current.Where(query));
        
        return results;
    }
}