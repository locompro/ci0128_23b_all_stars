using System.Linq.Expressions;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Models.Entities;

namespace Locompro.Common.Search.QueryBuilder;

/// <summary>
///     Builder class that constructs a list of search functions that can be used to filter the results of a query
/// </summary>
public class QueryBuilder<TSearchResults> : IQueryBuilder
{
    private readonly List<ISearchCriterion> _searchCriteria;
    private readonly List<Expression<Func<TSearchResults, bool>>> _searchCriteriaFunctions;
    private readonly ISearchMethods _searchMethods;

    /// <summary>
    ///     Constructor
    /// </summary>
    public QueryBuilder(ISearchMethods searchMethods)
    {
        _searchCriteria = new List<ISearchCriterion>();
        _searchCriteriaFunctions = new List<Expression<Func<TSearchResults, bool>>>();
        _searchMethods = searchMethods;
    }

    /// <inheritdoc />
    public void AddSearchCriterion(ISearchCriterion searchCriterion)
    {
        if (searchCriterion == null)
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                        + "Null search criterion passed");

        // if invalid parameter type then notify along with exception
        if (searchCriterion.ParameterName == default
            || !Enum.IsDefined(typeof(SearchParameterTypes), searchCriterion.ParameterName))
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                        + "Search criterion: " + nameof(searchCriterion.GetSearchValue));
        
        // the type provided should be consistent with the types that the search methods can handle
        if (searchCriterion.GetSearchValue() != null)
        {
            var isTypeConsistentWithMappedFunctions =
                IsTypeConsistentWithMappedFunctions(searchCriterion, out var searchValueType, out var searchQueryType);
            
            if (!isTypeConsistentWithMappedFunctions)
                throw new ArgumentException("Incompatible values used. SearchCriterion Type value: " + searchValueType
                    + "\nSearchQueryType value: " + searchQueryType);
        }

        _searchCriteria.Add(searchCriterion);
    }
    
    /// <inheritdoc />
    public ISearchQueries GetSearchFunction()
    {
        Compose();
        return new SearchQueries<TSearchResults>(_searchCriteriaFunctions);
    }

    /// <inheritdoc />
    public void Reset()
    {
        _searchCriteria.Clear();
        _searchCriteriaFunctions.Clear();
    }

    /// <summary>
    ///     Creates the list of search functions that can be used to filter the results of a query
    /// </summary>
    private void Compose()
    {
        // for each of the criterion in the unfiltered list
        foreach (var searchCriterion in _searchCriteria)
        {
            // get the search parameter that corresponds to the criterion
            var searchParameter = _searchMethods.GetSearchMethodByName(searchCriterion.ParameterName);
            
            if (searchParameter == null) continue;

            var activationQualifier = searchParameter.GetActivationQualifier();

            if (!activationQualifier.GetQualifierFunction()(searchCriterion.GetSearchValue())) continue;

            var expressionToAdd = GetExpressionToAdd(
                searchParameter.SearchQuery,
                searchCriterion);

            _searchCriteriaFunctions.Add(expressionToAdd);
        }
    }

    /// <summary>
    ///     Checks if the type provided in runtime coincide with the types that
    ///     the statically declared search methods can handle
    ///     The errors are however not runtime, these were errors that were not caught in compile-time
    /// </summary>
    /// <param name="searchCriterion"></param>
    /// <param name="searchValueType"> returns the type of the value given. Purely for error handling </param>
    /// <param name="searchQueryType"> returns the type of the searchquery. Purely for error handling</param>
    /// <returns> if consistent or not</returns>
    /// <exception cref="ArgumentException"></exception>
    private bool IsTypeConsistentWithMappedFunctions(
        ISearchCriterion searchCriterion,
        out string searchValueType,
        out string searchQueryType)
    {
        if (!_searchMethods.Contains(searchCriterion.ParameterName))
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                        + "Search criterion: " + nameof(searchCriterion.GetSearchValue));
        
        var searchMethod = _searchMethods.GetSearchMethodByName(searchCriterion.ParameterName);
        
        searchValueType = searchCriterion.GetSearchValue().GetType().Name;
        
        searchQueryType = searchMethod.SearchQuery.GetQueryFunction().Type.ToString();
        
        return searchQueryType.Contains(searchValueType);
    }

    /// <summary>
    ///     With the search query and the search value, create an expression that can be added to the list of search functions
    ///     that linq can translate properly to sql
    /// </summary>
    /// <param name="searchQuery"> the expression or function of how a parameter is to be searched</param>
    /// <param name="searchValue"> the string value to be compared </param>
    /// <returns></returns>
    private static Expression<Func<TSearchResults, bool>> GetExpressionToAdd(ISearchQuery searchQuery,
        ISearchCriterion searchValue)
    {
        // Create a parameter for the entity type
        var param = Expression.Parameter(typeof(TSearchResults), "x");

        // Get a constant expression for the string of the search value
        ConstantExpression searchValueExpression = Expression.Constant(searchValue.GetSearchValue());

        return Expression.Lambda<Func<TSearchResults, bool>>(
            Expression.Invoke(searchQuery.GetQueryFunction(), param, searchValueExpression),
            param
        );
    }
}