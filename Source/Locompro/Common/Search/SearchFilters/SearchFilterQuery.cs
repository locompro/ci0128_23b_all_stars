using System.Linq.Expressions;
using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Common.Search.SearchFilters;

public class SearchFilterQuery<TSearchResult, TSearchParameter> : ISearchFilterQuery
{
    private Func<TSearchResult, TSearchParameter, bool> QueryFunction { get; init; }

    public SearchFilterQuery(Func<TSearchResult, TSearchParameter, bool> queryFunction)
    {
        QueryFunction = queryFunction;
    }

    public Func<dynamic, dynamic, bool> GetQueryFunction()
    {
        return 
            (result, parameter) =>
                {
                    TSearchResult typedResult = (TSearchResult)result;
                    TSearchParameter typedParameter = (TSearchParameter)parameter;
                    
                    return QueryFunction(typedResult, typedParameter);
                };
    }
}