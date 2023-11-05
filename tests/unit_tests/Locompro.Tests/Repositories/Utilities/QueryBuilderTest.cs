using System.Linq.Expressions;
using System.Reflection;
using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Models;

namespace Locompro.Tests.Repositories.Utilities;

[TestFixture]
public class QueryBuilderTest
{
    private IQueryBuilder _queryBuilder;
    
    public QueryBuilderTest()
    {
        _queryBuilder = new QueryBuilder();
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
            _queryBuilder.AddSearchCriterion(searchCriterion);
        });
        
        // restore query builder state
        _queryBuilder.Reset();
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
        _queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQueries builtQueries = _queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.That(builtQueries.SearchQueryFunctions.Count, Is.EqualTo(1));
        
        // restore query builder state
        _queryBuilder.Reset();
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
        IQueryBuilder temp = _queryBuilder;
        _queryBuilder = new QueryBuilder();
        
        // Arrange
        ISearchCriterion searchCriterion = new SearchCriterion<string>
        {
            ParameterName = SearchParameterTypes.Name,
            SearchValue = "test"
        };
        
        _queryBuilder.AddSearchCriterion(searchCriterion);
        
        SearchQueries builtQueries = _queryBuilder.GetSearchFunction();
        
        int previousSize = builtQueries.SearchQueryFunctions.Count;
        
        // Act
        _queryBuilder.Reset();
        
        builtQueries = _queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(previousSize, Is.EqualTo(1));
            Assert.That(builtQueries.SearchQueryFunctions.Count, Is.EqualTo(0));
        });
        
        // restore state of query builder
        _queryBuilder = temp;
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
            _queryBuilder.AddSearchCriterion(searchCriterion);
        }
        
        SearchQueries builtQueries = _queryBuilder.GetSearchFunction();
        
        // Assert
        Assert.That(builtQueries.SearchQueryFunctions.Count, Is.EqualTo(3));
        
        _queryBuilder.Reset();
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

        ISearchCriterion? invalidSearchCriterionNull = null;
        
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
            _queryBuilder.AddSearchCriterion(validSearchCriterion);
            SearchQueries builtQueries = _queryBuilder.GetSearchFunction();
            Assert.That(builtQueries.SearchQueryFunctions, Has.Count.EqualTo(1));
            _queryBuilder.Reset();
            
            // Check that invalid null search criterion fails
            Assert.Throws<ArgumentException>(() =>
            {
                _queryBuilder.AddSearchCriterion(invalidSearchCriterionNull);
                builtQueries = this._queryBuilder.GetSearchFunction();
            });
            Assert.That(builtQueries.SearchQueryFunctions, Is.Empty);
            _queryBuilder.Reset();
            
            // Check that invalid empty search criterion fails
            Assert.Throws<ArgumentException>(() =>
            {
                _queryBuilder.AddSearchCriterion(invalidSearchCriterionEmpty);
                builtQueries = _queryBuilder.GetSearchFunction();
            });
            Assert.That(builtQueries.SearchQueryFunctions, Is.Empty);
            _queryBuilder.Reset();
            
            // Check that invalid search criterion with invalid parameter fails
            Assert.Throws<ArgumentException>(() =>
            {
                _queryBuilder.AddSearchCriterion(invalidSearchCriterionInvalidParameter);
                builtQueries = this._queryBuilder.GetSearchFunction();
            });
            Assert.That(builtQueries.SearchQueryFunctions, Is.Empty);
            _queryBuilder.Reset();
            
            // Check that invalid search criterion with default parameter fails
            Assert.Throws<ArgumentException>(() =>
            {
                _queryBuilder.AddSearchCriterion(invalidSearchCriterionDefault);
                builtQueries = _queryBuilder.GetSearchFunction();
            });
            Assert.That(builtQueries.SearchQueryFunctions, Is.Empty);
            _queryBuilder.Reset();
        });
    }
}