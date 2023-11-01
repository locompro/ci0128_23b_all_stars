using System.Linq.Expressions;
using System.Reflection;
using Locompro.Models;
using Locompro.SearchQueryConstruction;

namespace Locompro.Tests.Repositories.Utilities;

[TestFixture]
public class QueryBuilderTest
{
    private IQueryBuilder _queryBuilder;
    
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
        ISearchCriterion searchCriterion = new SearchCriterion<string>(default, "");
        
        Assert.Throws<ArgumentException>(() =>
        {
            this._queryBuilder.AddSearchCriterion(searchCriterion);
        });
        
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
         ISearchCriterion searchCriterion = new SearchCriterion<string>
        {
            ParameterName = SearchParameterTypes.Name,
            SearchValue = "test"
        };
        
        // Act
        this._queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQueries builtQueries = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.AreEqual(1, builtQueries.SearchQueryFunctions.Count);
        
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
        IQueryBuilder temp = this._queryBuilder;
        this._queryBuilder = new QueryBuilder();
        
        // Arrange
        ISearchCriterion searchCriterion = new SearchCriterion<string>
        {
            ParameterName = SearchParameterTypes.Name,
            SearchValue = "test"
        };
        
        this._queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQueries builtQueries = this._queryBuilder.GetSearchFunction();
        
        int previousSize = builtQueries.SearchQueryFunctions.Count;
        
        // Act
        this._queryBuilder.Reset();
        
        builtQueries = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.AreEqual(1, previousSize);
            Assert.AreEqual(0, builtQueries.SearchQueryFunctions.Count);
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
        List<ISearchCriterion> searchCriteria = new List<ISearchCriterion>()
        {
            new SearchCriterion<string>(SearchParameterTypes.Name, "name"),
            new SearchCriterion<string>(SearchParameterTypes.Province, "province"),
            new SearchCriterion<string>(SearchParameterTypes.Canton, "canton"),
        };
        
        // Act
        foreach (ISearchCriterion searchCriterion in searchCriteria)
        {
            this._queryBuilder.AddSearchCriterion(searchCriterion);
        }
        
        SearchQueries builtQueries = this._queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.AreEqual(3, builtQueries.SearchQueryFunctions.Count);
        
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
        ISearchCriterion validSearchCriterion = new SearchCriterion<string>
        {
            ParameterName = SearchParameterTypes.Name,
            SearchValue = "test"
        };

        ISearchCriterion invalidSearchCriterionNull = null;
        
        ISearchCriterion invalidSearchCriterionEmpty = new SearchCriterion<string>();
        
        ISearchCriterion invalidSearchCriterionInvalidParameter = new SearchCriterion<string>
        {
            ParameterName = (SearchParameterTypes) 100,
            SearchValue = "test"
        };
        
        ISearchCriterion invalidSearchCriterionDefault = new SearchCriterion<string>
        {
            ParameterName = default,
            SearchValue = "test"
        };

        
        Assert.Multiple(() =>
        {
            // Check that adding a valid search criterion returns true
            this._queryBuilder.AddSearchCriterion(validSearchCriterion);
            SearchQueries builtQueries = this._queryBuilder.GetSearchFunction();
            Assert.AreEqual(1, builtQueries.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid null search criterion fails
            Assert.Throws<ArgumentException>(() =>
            {
                this._queryBuilder.AddSearchCriterion(invalidSearchCriterionNull);
                builtQueries = this._queryBuilder.GetSearchFunction();
            });
            Assert.AreEqual(0, builtQueries.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid empty search criterion fails
            Assert.Throws<ArgumentException>(() =>
            {
                this._queryBuilder.AddSearchCriterion(invalidSearchCriterionEmpty);
                builtQueries = this._queryBuilder.GetSearchFunction();
            });
            Assert.AreEqual(0, builtQueries.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid search criterion with invalid parameter fails
            Assert.Throws<ArgumentException>(() =>
            {
                this._queryBuilder.AddSearchCriterion(invalidSearchCriterionInvalidParameter);
                builtQueries = this._queryBuilder.GetSearchFunction();
            });
            Assert.AreEqual(0, builtQueries.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
            
            // Check that invalid search criterion with default parameter fails
            Assert.Throws<ArgumentException>(() =>
            {
                this._queryBuilder.AddSearchCriterion(invalidSearchCriterionDefault);
                builtQueries = this._queryBuilder.GetSearchFunction();
            });
            Assert.AreEqual(0, builtQueries.SearchQueryFunctions.Count);
            _queryBuilder.Reset();
        });
    }
}