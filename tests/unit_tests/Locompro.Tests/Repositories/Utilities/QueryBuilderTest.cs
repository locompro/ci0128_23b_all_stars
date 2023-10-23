using System.Linq.Expressions;
using System.Reflection;
using Locompro.Models;
using Locompro.Repositories.Utilities;

namespace Locompro.Tests.Repositories.Utilities;

[TestFixture]
public class QueryBuilderTest
{
    private QueryBuilder _queryBuilder;
    
    public QueryBuilderTest()
    {
        this._queryBuilder = new QueryBuilder();
    }
    
    /// <summary>
    /// Tests that if an invalid criterion is added, it is not added to the query
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void AddInvalidSearchCriterion()
    {
        // Arrange
        SearchCriterion searchCriterion = new SearchCriterion();
        
        // Act
        this._queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQuery builtQuery = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.AreEqual(0, builtQuery.SearchQueryFunctions.Count);
        
        // restore query builder state
        this._queryBuilder.Reset();
    }
    
    /// <summary>
    /// Tests that if a valid criterion is added, it is added to the query
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void AddValidSearchCriterion()
    {
        // Arrange
        SearchCriterion searchCriterion = new SearchCriterion
        {
            ParameterName = SearchParam.SearchParameterTypes.Name,
            SearchValue = "test"
        };
        
        // Act
        this._queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQuery builtQuery = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.AreEqual(1, builtQuery.SearchQueryFunctions.Count);
        
        // restore query builder state
        this._queryBuilder.Reset();
    }
    
    /// <summary>
    /// Checks that the reset method resets the query builder
    /// Which means that the query builder is empty after the reset
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void ResetResetsSuccessfully()
    {
        // Ensure that state is empty for query builder in this test
        QueryBuilder temp = this._queryBuilder;
        this._queryBuilder = new QueryBuilder();
        
        // Arrange
        SearchCriterion searchCriterion = new SearchCriterion
        {
            ParameterName = SearchParam.SearchParameterTypes.Name,
            SearchValue = "test"
        };
        
        this._queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQuery builtQuery = this._queryBuilder.GetSearchFunction();
        
        int previousSize = builtQuery.SearchQueryFunctions.Count;
        
        // Act
        this._queryBuilder.Reset();
        
        builtQuery = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.AreEqual(1, previousSize);
            Assert.AreEqual(0, builtQuery.SearchQueryFunctions.Count);
        });
        
        // restore state of query builder
        this._queryBuilder = temp;
    }
    
    /// <summary>
    /// Checks that if multiple search criteria are added, they are all added to the query
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void AddMultipleSearchCriterion()
    {
        // Arrange
        List<SearchCriterion> searchCriteria = new List<SearchCriterion>()
        {
            new(SearchParam.SearchParameterTypes.Name, "name"),
            new(SearchParam.SearchParameterTypes.Province, "province"),
            new(SearchParam.SearchParameterTypes.Canton, "canton"),
        };
        
        // Act
        foreach (SearchCriterion searchCriterion in searchCriteria)
        {
            this._queryBuilder.AddSearchCriterion(searchCriterion);
        }
        
        SearchQuery builtQuery = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.AreEqual(3, builtQuery.SearchQueryFunctions.Count);
        
        this._queryBuilder.Reset();
    }
    
    /// <summary>
    /// Checks if the AddSearchCriterion method returns the correct outputs
    /// Tests for a valid search criterion,
    /// an invalid null search criterion,
    /// an invalid empty search criterion,
    /// an invalid search criterion with an invalid parameter and an invalid search criterion with a default parameter
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void AddSearchCriterionReturnsValidOutputs()
    {
        // Arrange
        SearchCriterion validSearchCriterion = new SearchCriterion
        {
            ParameterName = SearchParam.SearchParameterTypes.Name,
            SearchValue = "test"
        };

        SearchCriterion invalidSearchCriterionNull = null;
        
        SearchCriterion invalidSearchCriterionEmpty = new SearchCriterion();
        
        SearchCriterion invalidSearchCriterionInvalidParameter = new SearchCriterion
        {
            ParameterName = (SearchParam.SearchParameterTypes) 100,
            SearchValue = "test"
        };
        
        SearchCriterion invalidSearchCriterionDefault = new SearchCriterion
        {
            ParameterName = default,
            SearchValue = "test"
        };

        
        Assert.Multiple(() =>
        {
            // Check that adding a valid search criterion returns true
            this._queryBuilder.AddSearchCriterion(validSearchCriterion);
            SearchQuery builtQuery = this._queryBuilder.GetSearchFunction();
            Assert.AreEqual(1, builtQuery.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid null search criterion fails
            this._queryBuilder.AddSearchCriterion(invalidSearchCriterionNull);
            builtQuery = this._queryBuilder.GetSearchFunction();
            Assert.AreEqual(0, builtQuery.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid empty search criterion fails
            this._queryBuilder.AddSearchCriterion(invalidSearchCriterionEmpty);
            builtQuery = this._queryBuilder.GetSearchFunction();
            Assert.AreEqual(0, builtQuery.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid search criterion with invalid parameter fails
            this._queryBuilder.AddSearchCriterion(invalidSearchCriterionInvalidParameter);
            builtQuery = this._queryBuilder.GetSearchFunction();
            Assert.AreEqual(0, builtQuery.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid search criterion with default parameter fails
            this._queryBuilder.AddSearchCriterion(invalidSearchCriterionDefault);
            builtQuery = this._queryBuilder.GetSearchFunction();
            Assert.AreEqual(0, builtQuery.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
        });
    }
}