using System.Linq.Expressions;
using Locompro.Models.Entities;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Common.Search.SearchMethodRegistration;

/// <summary>
///     Generic search query
/// </summary>
/// <typeparam name="TSearchParameter"></typeparam>
/// <typeparam name="TSearchResult"></typeparam>
public class SearchQuery<TSearchResult, TSearchParameter> : ISearchQuery
{
    private Expression<Func<TSearchResult, TSearchParameter, bool>> QueryFunction { get; init; }
    
    public SearchQuery(Expression<Func<TSearchResult, TSearchParameter, bool>> queryFunction)
    {
        QueryFunction = queryFunction;
    }
    
    /// <summary>
    ///     returns internal search query expression
    /// </summary>
    /// <returns></returns>
    public Expression GetQueryFunction()
    {
        return QueryFunction;
    }
    
    
}