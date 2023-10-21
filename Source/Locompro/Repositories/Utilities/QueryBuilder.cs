using System.Linq.Expressions;

namespace Locompro.Repositories.Utilities;
using System.Collections.Generic;
using Locompro.Models;

/// <summary>
/// Builder class that constructs a list of search functions that can be used to filter the results of a query
/// </summary>
public class QueryBuilder
{
    private readonly List<Expression<Func<Submission, bool>>> _searchCriteriaFunctions;
    private readonly List<SearchCriterion> _searchCriteria;
    
    /// <summary>
    /// Constructor
    /// </summary>
    public QueryBuilder()
    {
        this._searchCriteria = new List<SearchCriterion>();
        this._searchCriteriaFunctions = new List<Expression<Func<Submission, bool>>>();
    }

    /// <summary>
    /// Adds a search criterion to the list of search criteria
    /// A search criterion is a search parameter and a search value
    /// </summary>
    /// <param name="parameterName"> determines the type of search to be done</param>
    /// <param name="parameterValue"> the value to be searched </param>
    /// <param name="searchCriterion"></param>
    public void AddSearchCriterion(SearchCriterion searchCriterion)
    {
        // if the criterion is valid
        if (searchCriterion != null && !string.IsNullOrEmpty(searchCriterion.SearchValue))
        {
            this._searchCriteria.Add(searchCriterion);
        }
    }
    
    /// <summary>
    /// Creates the list of search functions that can be used to filter the results of a query
    /// </summary>
    private void Compose()
    {
        // for each of the criterion in the unfiltered list
        foreach (SearchCriterion searchCriterion in _searchCriteria)
        {
            // get the search parameter that corresponds to the criterion
            SearchParam searchParameter = SearchMethods.GetInstance.getSearchMethodByName(searchCriterion.ParameterName);
            
            // if the search parameter was found and complies with ActivationQualifier
            if (searchParameter != null && searchParameter.ActivationQualifier(searchCriterion.SearchValue))
            {
                // add the search function to the list of search functions
                this._searchCriteriaFunctions.Add(QueryBuilder.GetExpressionToAdd(searchParameter.SearchQuery, searchCriterion.SearchValue));
            }
        }
    }

    /// <summary>
    /// With the search query and the search value, create an expression that can be added to the list of search functions
    /// that linq can translate properly to sql
    /// </summary>
    /// <param name="searchQuery"> the expression or function of how a parameter is to be searched</param>
    /// <param name="searchValue"> the string value to be compared </param>
    /// <returns></returns>
    private static Expression<Func<Submission, bool>> GetExpressionToAdd(Expression<Func<Submission,string,bool>> searchQuery, string searchValue)
    {
        // Create a parameter for the entity type
        ParameterExpression param = Expression.Parameter(typeof(Submission), "x");

        // Get a constant expression for the string of the search value
        ConstantExpression searchValueExpression = Expression.Constant(searchValue);
        
        return Expression.Lambda<Func<Submission, bool>>(
            Expression.Invoke(searchQuery, param, searchValueExpression),
            param
        );
    }
    
    /// <summary>
    /// Returns the list of search functions that can be used to filter the results of a query
    /// </summary>
    /// <returns></returns>
    public SearchQuery GetSearchFunction()
    {
        this.Compose();
        return new SearchQuery() { searchQueryFunctions = this._searchCriteriaFunctions };
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