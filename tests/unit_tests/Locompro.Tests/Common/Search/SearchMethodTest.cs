using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

namespace Locompro.Tests.Common.Search;

public class SearchMethodTest
{
    /// <summary>
    ///     Checks if a that when an invalid search type is provided, it returns null
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 2</author>
    /// </summary>
    [Test]
    public void GetSearchMethodByNameReturnsNull()
    {
        // Arrange
        var searchMethods = SubmissionSearchMethods.GetInstance();
 
        // Act
        var searchParam = searchMethods. GetSearchMethodByName(SearchParameterTypes.Default);

        // Assert
        Assert.IsNull(searchParam);
    }

    /// <summary>
    ///     Checks if the valid search types return a non null search parameter which means it has been found
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 2</author>
    /// </summary>
    [Test]
    public void GetSearchMethodByNameReturnsNotNull()
    {
        // Arrange
        var searchMethods = SubmissionSearchMethods.GetInstance();

        Assert.Multiple(() =>
        {
            var searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.SubmissionByName);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.SubmissionByProvince);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.SubmissionByCanton);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.SubmissionByModel);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.SubmissionByBrand);
            Assert.That(searchParam, Is.Not.Null);

            searchParam = searchMethods.GetSearchMethodByName(SearchParameterTypes.SubmissionByCategory);
            Assert.That(searchParam, Is.Not.Null);
        });
    }
}