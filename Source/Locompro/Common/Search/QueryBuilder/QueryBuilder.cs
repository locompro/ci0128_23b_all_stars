using System.Linq.Expressions;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Models;

namespace Locompro.Common.Search.QueryBuilder;

/// <summary>
/// Builder class that constructs a list of search functions that can be used to filter the results of a query
/// </summary>
public class QueryBuilder : IQueryBuilder
{
    private readonly List<Expression<Func<Submission, bool>>> _searchCriteriaFunctions;
    private readonly List<ISearchCriterion> _searchCriteria;
    
    /// <summary>
    /// Constructor
    /// </summary>
    public QueryBuilder()
    {
        _searchCriteria = new List<ISearchCriterion>();
        _searchCriteriaFunctions = new List<Expression<Func<Submission, bool>>>();
    }

    /// <summary>
    /// Adds a search criterion to the list of search criteria
    /// A search criterion is a search parameter and a search value
    /// </summary>
    /// <param name="searchCriterion"></param>
    public void AddSearchCriterion(ISearchCriterion searchCriterion)
    {
        if (searchCriterion == null)
        {
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                               + "Null search criterion passed");
        }
        
        // if invalid parameter type then notify along with exception
        if (searchCriterion.ParameterName == default
            || !Enum.IsDefined(typeof(SearchParameterTypes), searchCriterion.ParameterName))
        {
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                               + "Search criterion: " + nameof(searchCriterion.GetSearchValue));
        }
        
        // the type provided should be consistent with the types that the search methods can handle
        if (searchCriterion.GetSearchValue() != null)
        {
            bool isTypeConsistentWithMappedFunctions = IsTypeConsistentWithMappedFunctions(searchCriterion, out string searchValueType, out string searchQueryType);
            if (!isTypeConsistentWithMappedFunctions)
            {
                throw new ArgumentException("Incompatible values used. SearchCriterion Type value: " + searchValueType
                    + "\nSearchQueryType value: " + searchQueryType);
            } 
        }
        
        this._searchCriteria.Add(searchCriterion);
    }
    
    /// <summary>
    /// Creates the list of search functions that can be used to filter the results of a query
    /// </summary>
    private void Compose()
    {
        // for each of the criterion in the unfiltered list
        foreach (ISearchCriterion searchCriterion in _searchCriteria)
        {
            // get the search parameter that corresponds to the criterion
            SearchParam searchParameter = SearchMethods.GetInstance.GetSearchMethodByName(searchCriterion.ParameterName);

            if (searchParameter == null)
            {
                continue;
            }

            IActivationQualifier activationQualifier = searchParameter.GetActivationQualifier();
            
            if (!activationQualifier.GetQualifierFunction()(searchCriterion.GetSearchValue())) continue;
            
            Expression<Func<Submission, bool>> expressionToAdd = QueryBuilder.GetExpressionToAdd(
                searchParameter.SearchQuery,
                searchCriterion);
                
            this._searchCriteriaFunctions.Add(expressionToAdd);
        }
    }
    
    /// <summary>
    /// Checks if the type provided in runtime coincide with the types that
    /// the statically declared search methods can handle
    /// The errors are however not runtime, these were errors that were not caught in compile-time
    /// </summary>
    /// <param name="searchCriterion"></param>
    /// <param name="searchValueType"> returns the type of the value given. Purely for error handling </param>
    /// <param name="searchQueryType"> returns the type of the searchquery. Purely for error handling</param>
    /// <returns> if consistent or not</returns>
    /// <exception cref="ArgumentException"></exception>
    private static bool IsTypeConsistentWithMappedFunctions(
        ISearchCriterion searchCriterion,
        out string searchValueType,
        out string searchQueryType)
    {
        if (!SearchMethods.GetInstance.Contains(searchCriterion.ParameterName))
        {
            throw new ArgumentException("Invalid search criterion addition attempt\n"
                                        + "Search criterion: " + nameof(searchCriterion.GetSearchValue));
        }
        
        SearchParam searchMethod = SearchMethods.GetInstance.GetSearchMethodByName(searchCriterion.ParameterName);
        
        searchValueType = searchCriterion.GetSearchValue().GetType().Name;
        searchQueryType = searchMethod.SearchQuery.GetQueryFunction().Type.ToString();
        
        return searchQueryType.Contains(searchValueType);
    }

    /// <summary>
    /// With the search query and the search value, create an expression that can be added to the list of search functions
    /// that linq can translate properly to sql
    /// </summary>
    /// <param name="searchQuery"> the expression or function of how a parameter is to be searched</param>
    /// <param name="searchValue"> the string value to be compared </param>
    /// <returns></returns>
    private static Expression<Func<Submission, bool>> GetExpressionToAdd(ISearchQuery searchQuery, ISearchCriterion searchValue)
    {
        // Create a parameter for the entity type
        ParameterExpression param = Expression.Parameter(typeof(Submission), "x");

        // Get a constant expression for the string of the search value
        ConstantExpression searchValueExpression = Expression.Constant(searchValue.GetSearchValue());
        
        return Expression.Lambda<Func<Submission, bool>>(
            Expression.Invoke(searchQuery.GetQueryFunction(), param, searchValueExpression),
            param
        );
    }
    
    /// <summary>
    /// Returns the list of search functions that can be used to filter the results of a query
    /// </summary>
    /// <returns></returns>
    public SearchQueries GetSearchFunction()
    {
        this.Compose();
        return new SearchQueries() { SearchQueryFunctions = this._searchCriteriaFunctions };
    }
    
    /// <summary>
    /// Resets the builder class
    /// </summary>
    public void Reset()
    {
        this._searchCriteria.Clear();
        _searchCriteriaFunctions.Clear();
    }
}