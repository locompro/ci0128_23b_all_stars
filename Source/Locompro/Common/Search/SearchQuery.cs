using System.Linq.Expressions;
using Locompro.Common.Search.Interfaces;
using Locompro.Models;

namespace Locompro.Common.Search;

/// <summary>
/// Generic search query
/// </summary>
/// <typeparam name="T"></typeparam>
public class SearchQuery<T> : ISearchQuery
{
    public Expression<Func<Submission, T, bool>> QueryFunction { get; init; }
    
    /// <summary>
    /// returns internal search query expression
    /// </summary>
    /// <returns></returns>
    public Expression GetQueryFunction()
    {
        return QueryFunction;
    }
}