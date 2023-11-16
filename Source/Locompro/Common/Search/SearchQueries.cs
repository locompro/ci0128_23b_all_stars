using System.Linq.Expressions;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Models.Entities;

namespace Locompro.Common.Search;

/// <summary>
///     Class for encapsulating all data related to a search criterion
///     If there were other types of criteria or functions to be used, then add to this class
/// </summary>
public class SearchQueries<TSearchResult> : ISearchQueries
{
    private readonly List<Expression<Func<TSearchResult, bool>>> _searchQueryFunctions;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchQueryFunctions"> search query to be stored </param>
    public SearchQueries(List<Expression<Func<TSearchResult, bool>>> searchQueryFunctions)
    {
        _searchQueryFunctions = searchQueryFunctions;
    }
    
    /// <inheritdoc />
    public bool IsEmpty() => _searchQueryFunctions.Count == 0;

    /// <inheritdoc />
    public int Count()
    {
        return _searchQueryFunctions.Count;
    }
    
    /// <inheritdoc />
    public IQueryable ApplySearch(IQueryable queryable)
    {
        IQueryable<TSearchResult> results = queryable as IQueryable<TSearchResult>;
        results = 
            _searchQueryFunctions.Aggregate(
                results ,(current, query) =>current.Where(query));

       return results;
    }
}