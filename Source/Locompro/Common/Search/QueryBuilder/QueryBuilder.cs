#nullable enable
using System.Linq.Expressions;
using Locompro.Common.Search.SearchFilters;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Models.Entities;

namespace Locompro.Common.Search.QueryBuilder;

/// <summary>
///     Builder class that constructs a list of search functions that can be used to filter the results of a query
/// </summary>
public class QueryBuilder<TSearchResults> : IQueryBuilder<TSearchResults>
{
    private readonly ILogger _logger;
    private readonly ISearchMethods _searchMethods;
    
    private readonly List<ISearchCriterion> _searchCriteria;
    private readonly List<ISearchCriterion> _searchFilters;
    
    private readonly List<Expression<Func<TSearchResults, bool>>> _searchCriteriaFunctions;
    private readonly List<Func<TSearchResults, bool>> _searchFilterFunctions;
    
    private readonly List<Expression<Func<TSearchResults, bool>>> _uniqueSearchExpressions;
    
    /// <summary>
    ///     Constructor
    /// </summary>
    public QueryBuilder(ISearchMethods searchMethods, ILogger logger)
    {
        _logger = logger;
        _searchMethods = searchMethods;
        
        _searchCriteria = new List<ISearchCriterion>();
        _searchFilters = new List<ISearchCriterion>();
        
        _searchCriteriaFunctions = new List<Expression<Func<TSearchResults, bool>>>();
        _searchFilterFunctions = new List<Func<TSearchResults, bool>>();
        
        _uniqueSearchExpressions = new List<Expression<Func<TSearchResults, bool>>>();
    }
    
    /// <inheritdoc />
    public void AddSearchCriteria(ISearchQueryParameters<TSearchResults> searchQueryParameters)
    {
        foreach (var searchCriterion in searchQueryParameters.GetQueryParameters())
        {
            try
            {
                AddSearchCriterion(searchCriterion);
            } catch (ArgumentException e)
            {
                _logger.LogError(e, "Failed to add search criterion");
            }
            
        }

        foreach (var searchFilter in searchQueryParameters.GetSearchFilters())
        {
            try
            {
                AddFilter(searchFilter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to add search filter");
            }
        }
        
        foreach (var uniqueSearchExpression in searchQueryParameters.GetUniqueSearchExpressions())
        {
            _uniqueSearchExpressions.Add(uniqueSearchExpression);
        }
    }
    
    /// <inheritdoc />
    public ISearchQueries<TSearchResults> GetSearchFunction()
    {
        Compose();
        return new SearchQueries<TSearchResults>(
            _searchCriteriaFunctions,
            _searchFilterFunctions,
            _uniqueSearchExpressions);
    }

    /// <inheritdoc />
    public void Reset()
    {
        _searchCriteria.Clear();
        _searchCriteriaFunctions.Clear();
        _searchFilters.Clear();
        _searchFilterFunctions.Clear();
        _uniqueSearchExpressions.Clear();
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
        {
            var value = searchCriterion.GetSearchValue();
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                        + "Search criterion: " + nameof(value));
        }
        
        dynamic? criterionValue = searchCriterion.GetSearchValue();
        
        // the type provided should be consistent with the types that the search methods can handle
        if (criterionValue is not null)
        {
            var isTypeConsistentWithMappedFunctions =
                IsTypeConsistentWithMappedFunctions(searchCriterion, out var searchValueType, out var searchQueryType);
            
            if (!isTypeConsistentWithMappedFunctions)
                throw new ArgumentException("Incompatible values used. SearchCriterion Type value: " + searchValueType
                    + "\nSearchQueryType value: " + searchQueryType);
        }

        _searchCriteria.Add(searchCriterion);
    }

    private void AddFilter(ISearchCriterion filter)
    {
        if (filter == null)
            throw new ArgumentException("Invalid search filter addition attempt\n"
                                        + "Null search filter passed");

        // if invalid parameter type then notify along with exception
        if (filter.ParameterName == default
            || !Enum.IsDefined(typeof(SearchParameterTypes), filter.ParameterName))
            throw new ArgumentException("Invalid search filter addition attempt\n"
                                        + "Search filter: " + nameof(filter.GetSearchValue));
        
        dynamic? filterValue = filter.GetSearchValue();
        
        // the type provided should be consistent with the types that the search methods can handle
        if (filterValue is not null)
        {
            var isTypeConsistentWithMappedFunctions =
                IsFilterTypeConsistentWithMappedFunctions(filter, out var searchValueType, out var searchQueryType);
            
            if (!isTypeConsistentWithMappedFunctions)
                throw new ArgumentException("Incompatible values used. SearchCriterion Type value: " + searchValueType
                    + "\nSearchQueryType value: " + searchQueryType);
        }
        
        _searchFilters.Add(filter);
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

        foreach (var filter in _searchFilters) 
        {
            ISearchFilterParam? searchFilter = _searchMethods.GetSearchFilterByName(filter.ParameterName);
            
            if (searchFilter == null) continue;
            
            IActivationQualifier? activationQualifier = searchFilter.GetActivationQualifier();
            
            if (!activationQualifier.GetQualifierFunction()(filter.GetSearchValue())) continue;
            
            Func<TSearchResults,bool>? filterToAdd = GetFilterToAdd(
                searchFilter.GetSearchQuery(),
                filter);
            
            _searchFilterFunctions.Add(filterToAdd);
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
                                        + "Search criterion: " + searchCriterion.ParameterName.ToString());
        
        var searchMethod = _searchMethods.GetSearchMethodByName(searchCriterion.ParameterName);
        
        searchValueType = searchCriterion.GetSearchValue().GetType().Name;
        
        searchQueryType = searchMethod.SearchQuery.GetQueryFunction().Type.ToString();
        
        return searchQueryType.Contains(searchValueType);
    }

    private bool IsFilterTypeConsistentWithMappedFunctions(
        ISearchCriterion searchCriterion,
        out string searchValueType,
        out string searchQueryType)
    {
        if (!_searchMethods.ContainsSearchFilter(searchCriterion.ParameterName))
            throw new ArgumentException("Invalid search filter addition attempt\n"
                                        + "Search filter: " + nameof(searchCriterion.GetSearchValue));
        
        var searchMethod = _searchMethods.GetSearchFilterByName(searchCriterion.ParameterName);
        
        searchValueType = searchCriterion.GetSearchValue().GetType().Name;
        searchQueryType = searchMethod.GetSearchQuery().GetQueryFunction().GetType().ToString();
        
        return true;
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
        
        Expression predicate = searchQuery.GetQueryFunction();
        
        InvocationExpression? body = Expression.Invoke(predicate, param, searchValueExpression);

        return Expression.Lambda<Func<TSearchResults, bool>>(
            body,
            param
        );
    }

    private static Func<TSearchResults, bool> GetFilterToAdd(ISearchFilterQuery searchFilter, ISearchCriterion filterValue)
    {
        Func<TSearchResults, dynamic, bool>? queryFunction = searchFilter.GetQueryFunction() as Func<TSearchResults, dynamic, bool>;
        var value = filterValue.GetSearchValue();
        
        Func<TSearchResults, bool>? constructedFilter = searchResult => queryFunction(searchResult, value);
        
        return constructedFilter;
    }
}