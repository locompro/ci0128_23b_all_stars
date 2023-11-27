using System.Linq.Expressions;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Expression = Castle.DynamicProxy.Generators.Emitters.SimpleAST.Expression;

namespace Locompro.Common.Search.SearchQueryParameters;

public class SearchQueryParameters<TSearchResult> : ISearchQueryParameters<TSearchResult>
{
    private readonly List<ISearchCriterion> _queryParameters;
    private readonly List<ISearchCriterion> _searchFilters;
    private readonly List<Expression<Func<TSearchResult, bool>>> _uniqueSearchExpressions;

    public SearchQueryParameters()
    {
        _queryParameters = new List<ISearchCriterion>();
        _searchFilters = new List<ISearchCriterion>();
        _uniqueSearchExpressions = new List<Expression<Func<TSearchResult, bool>>>();
    }

    public ISearchQueryParameters<TSearchResult> AddQueryParameter<TSearchParameter>(SearchParameterTypes searchParameterType, TSearchParameter parameter)
    {
        _queryParameters.Add(new SearchCriterion<TSearchParameter>(searchParameterType, parameter));
        return this;
    }

    public ISearchQueryParameters<TSearchResult> AddFilterParameter<TSearchParameter>(SearchParameterTypes searchParameterType,
        TSearchParameter parameter)
    {
        _searchFilters.Add(new SearchCriterion<TSearchParameter>(searchParameterType, parameter));

        return this;
    }

    public ISearchQueryParameters<TSearchResult> AddUniqueSearch<TSearchParameter>(
        Expression<Func<TSearchResult, bool>> uniqueSearchExpression,
        Func<TSearchParameter, bool> activationQualifier,
        TSearchParameter parameter)
    {
        if (!activationQualifier(parameter))
        {
            return this;
        }
        
        _uniqueSearchExpressions.Add(uniqueSearchExpression);
        
        return this;
    }

    public List<ISearchCriterion> GetQueryParameters()
    {
        return _queryParameters;
    }

    public List<ISearchCriterion> GetSearchFilters()
    {
        return _searchFilters;
    }
    
    public List<Expression<Func<TSearchResult, bool>>> GetUniqueSearchExpressions()
    {
        return _uniqueSearchExpressions;
    }

    public void Clear()
    {
        _queryParameters.Clear();
        _searchFilters.Clear();
    }
}