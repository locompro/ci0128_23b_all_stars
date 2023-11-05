
using Locompro.Common.Search;

namespace Locompro.Tests.Repositories.Utilities;

public class SearchMethodTest
{
    /// <summary>
    /// Checks if a that when an invalid search type is provided, it returns null
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void GetSearchMethodByNameReturnsNull()
    {
        // Arrange
        SearchMethods searchMethods = SearchMethods.GetInstance;
        
        // Act
        SearchParam searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Default);
        
        // Assert
        Assert.IsNull(searchParam);
    }
    
    /// <summary>
    /// Checks if the valid search types return a non null search parameter which means it has been found
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void GetSearchMethodByNameReturnsNotNull()
    {
        // Arrange
        SearchMethods searchMethods = SearchMethods.GetInstance;
        
        Assert.Multiple(() =>
        {
            SearchParam searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Name);
            Assert.IsNotNull(searchParam);
            
            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Province);
            Assert.IsNotNull(searchParam);
            
            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Canton);
            Assert.IsNotNull(searchParam);
            
            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Model);
            Assert.IsNotNull(searchParam);
            
            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Brand);
            Assert.IsNotNull(searchParam);
            
            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Category);
            Assert.IsNotNull(searchParam);
        });
    }
}