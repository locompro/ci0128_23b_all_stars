using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Tests.Common.Search;

public class SearchMethodTest
{
    /// <summary>
    ///     Checks if a that when an invalid search type is provided, it returns null
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void GetSearchMethodByNameReturnsNull()
    {
        // Arrange
        var searchMethods = SearchMethods.GetInstance;

        // Act
        var searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Default);

        // Assert
        Assert.IsNull(searchParam);
    }

    /// <summary>
    ///     Checks if the valid search types return a non null search parameter which means it has been found
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void GetSearchMethodByNameReturnsNotNull()
    {
        // Arrange
        var searchMethods = SearchMethods.GetInstance;

        Assert.Multiple(() =>
        {
            var searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Name);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Province);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Canton);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Model);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Brand);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.Category);
            Assert.That(searchParam, Is.Not.Null);
        });
    }
}