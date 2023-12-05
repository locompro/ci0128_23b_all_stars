using System.Linq.Expressions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

namespace Locompro.Common.Search;

public interface ISearchQueryParameters<TSearchResult>
{
    ISearchQueryParameters<TSearchResult> AddQueryParameter<TSearchParameter>(SearchParameterTypes searchParameterType, TSearchParameter parameter);
    
    ISearchQueryParameters<TSearchResult> AddFilterParameter<TSearchParameter>(SearchParameterTypes searchParameterType, TSearchParameter parameter);

    ISearchQueryParameters<TSearchResult> AddUniqueSearch<TSearchParameter>(
        Expression<Func<TSearchResult, bool>> uniqueSearchExpression,
        Func<TSearchParameter, bool> activationQualifier,
        TSearchParameter parameter);
    
    List<ISearchCriterion> GetQueryParameters();
    
    List<ISearchCriterion> GetSearchFilters();
    
    List<Expression<Func<TSearchResult, bool>>> GetUniqueSearchExpressions();

    void Clear();
}