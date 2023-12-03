using System.Globalization;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Locompro.Common.Mappers;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Common.Search.SearchQueryParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Types;
using Moq;
using NetTopologySuite.Geometries;

namespace Locompro.Tests.Services;

[TestFixture]
public class SearchServiceTest
{
    [SetUp]
    public void Setup()
    {
        ILoggerFactory loggerFactoryMock = new LoggerFactory();

        _submissionRepositoryMock = new Mock<ICrudRepository<Submission, SubmissionKey>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.GetCrudRepository<Submission, SubmissionKey>())
            .Returns(_submissionRepositoryMock.Object);

        IDomainService<Submission, SubmissionKey> searchDomainService =
            new DomainService<Submission, SubmissionKey>(_unitOfWorkMock.Object, loggerFactoryMock);

        _searchService = new SearchService(loggerFactoryMock, searchDomainService, null);
    }

    private Mock<ICrudRepository<Submission, SubmissionKey>>? _submissionRepositoryMock;
    private Mock<IUnitOfWork>? _unitOfWorkMock;
    private SearchService? _searchService;

    /// <summary>
    ///     Finds a list of names that are expected to be found
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task SearchByName_NameIsFound()
    {
        // Arrange
        var productSearchName = "Product1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);
        
        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);

        productSearchName = "Product2";

        searchCriteria.Clear();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);


        productSearchName = "Product3";
        searchCriteria.Clear();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);

        productSearchName = "Product4";
        searchCriteria.Clear();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);

        productSearchName = "Product5";
        searchCriteria.Clear();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);

        productSearchName = "Product6";
        searchCriteria.Clear();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);

        productSearchName = "Product7";
        searchCriteria.Clear();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.True);
    }

    /// <summary>
    ///     Searches for name that is not expected to be found and
    ///     returns empty list
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task SearchByNameForNonExistent_NameIsNotFound()
    {
        // Arrange
        var productSearchName = "ProductNonExistent";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.False);
            Assert.That(searchResults, Is.Empty);
        });
    }

    /// <summary>
    ///     Gives an empty string and returns empty list
    ///     Should not throw an exception
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task SearchByNameForEmptyString_NameIsNotFound()
    {
        // Arrange
        var productSearchName = "";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);
        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(searchResults.Exists(i => i.Name == productSearchName), Is.False);
            Assert.That(searchResults, Is.Empty);
        });
    }

    /// <summary>
    ///     Searches for an item and the result is the one expected
    ///     to be the best submission
    ///     <remarks>
    ///         This test is to be changed accordingly when
    ///         a new heuristic change to the best submission algorithm is made
    ///     </remarks>
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task SearchByName_FindsBestSubmission()
    {
        var productSearchName = "Product1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        var dateTimeExpected = new DateTime(2023, 10, 6, 0, 0, 0, DateTimeKind.Utc);
        var dateTimeReceived = DateTime.Parse(searchResults[0].LastSubmissionDate, new CultureInfo("en-US"));

        // Assert
        Assert.That(dateTimeReceived, Is.EqualTo(dateTimeExpected));
    }

    /// <summary>
    ///     Tests that the number of submissions given by the search are correct
    ///     <author>Gabriel Molina Bulgarelli C14826</author>
    /// </summary>
    [Test]
    public async Task AmountOfSearchSubmissions()
    {
        var productSearchName = "Product1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults, Is.Not.Empty);
        Assert.That(searchResults[0].Submissions, Has.Count.EqualTo(2));
    }

    /// <summary>
    ///     Tests that the data given on submissions given by a search are correctly assigned
    ///     <author>Gabriel Molina Bulgarelli C14826</author>
    /// </summary>
    [Test]
    public async Task DataOfSearchSubmissions()
    {
        var productSearchName = "Product1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByName, productSearchName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults, Is.Not.Empty);
        Assert.That(searchResults[0].Submissions, Has.Count.EqualTo(2));

        var submission1EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(submission1EntryTime.ToString(CultureInfo.InvariantCulture),
            Does.Contain(searchResults[0].Submissions[0].EntryTime));
        Assert.Multiple(() =>
        {
            Assert.That(searchResults[0].Submissions[0].Price, Is.EqualTo(100));
            Assert.That(searchResults[0].Submissions[0].Description, Is.EqualTo("Description for Submission 1"));
        });
        var submission2EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(submission2EntryTime.ToString(CultureInfo.InvariantCulture),
            Does.Contain(searchResults[0].Submissions[1].EntryTime));
        Assert.Multiple(() =>
        {
            Assert.That(searchResults[0].Submissions[1].Price, Is.EqualTo(180));
            Assert.That(searchResults[0].Submissions[1].Description, Is.EqualTo("Description for Submission 8"));
        });
    }

    /// <summary>
    ///     Searches for an item with a specific model and the result is the one expected
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 </author>
    [Test]
    public async Task SearchByModel_ModelIsFound()
    {
        // Arrange
        var modelName = "Model1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByModel, modelName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(searchResults, Is.Not.Empty);
            Assert.That(searchResults.TrueForAll(item => item.Model.Contains(modelName)),
                Is.True); // Verify that all items have the expected model name
        });
    }

    /// <summary>
    ///     Tests that the correct submissions result data is returned when the model is specified in search
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 </author>
    [Test]
    public async Task SubmissionsDataByModel()
    {
        // Arrange
        var modelName = "Model2";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByModel, modelName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);
        
        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults, Is.Not.Empty);
        Assert.Multiple(() =>
        {
            Assert.That(searchResults.TrueForAll(item => item.Model.Contains(modelName)),
                Is.True);
            Assert.That(searchResults[0].Submissions, Has.Count.EqualTo(1));

            Assert.That(searchResults[0].Submissions[0].Price, Is.EqualTo(200));
        });
        Assert.That(searchResults[0].Submissions[0].Description, Is.EqualTo("Description for Submission 2"));
    }

    /// <summary>
    ///     Searches for an item with a specific model and the result is empty
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 </author>
    [Test]
    public async Task SearchByModel_ModelIsNotFound()
    {
        // Arrange
        var modelName = "NonExistentModel";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByModel, modelName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults, Is.Empty); // Expecting an empty result
    }

    /// <summary>
    ///     Searches for an empty model and the result is empty, according to the expected behavior
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 </author>
    [Test]
    public async Task SearchByModel_EmptyModelName()
    {
        // Arrange
        var modelName = string.Empty;

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByModel, modelName);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults, Is.Empty); // Expecting an empty result
    }

    /// <summary>
    ///     tests that the search by canton and province returns the expected results when the canton and province are
    ///     mentioned in the submissions
    ///     <author> A. Badilla Olivas B80874 </author>
    /// </summary>
    [Test]
    public async Task GetSubmissionsByCantonAndProvince_ValidCantonAndProvince_SubmissionsReturned()
    {
        // Arrange
        var canton = "Canton1";
        var province = "Province1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByProvince, province)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCanton, canton);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Not.Empty);

        var all = true;

        foreach (var item in results)
            if (item.Canton != canton || item.Province != province)
                all = false;

        Assert.That(all, Is.True);
    }

    /// <summary>
    ///     Tests that an empty list is returned when the canton and province are not mentioned in any submission
    ///     <author> A. Badilla Olivas B80874 </author>
    /// </summary>
    [Test]
    public async Task GetSubmissionsByCantonAndProvince_InvalidCantonAndProvince_EmptyListReturned()
    {
        // Arrange
        var canton = "InvalidCanton";
        var province = "InvalidProvince";
        MockDataSetup();

        // Act
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByProvince, province)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCanton, canton);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(0));
    }

    /// <summary>
    ///     Tests that the correct result is returned when the brand is specified in search
    /// </summary>
    /// <author> Brandon Mora Umaña C15179 </author>
    [Test]
    public async Task GetSubmissionsByBrand_ValidBrand_SubmissionsReturned()
    {
        // Arrange
        var brand = "Brand1";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brand);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.TrueForAll(item => item.Brand.Contains(brand)), Is.True);
    }

    /// <summary>
    ///     Tests that the correct submissions result data is returned when the brand is specified in search
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826</author>
    [Test]
    public async Task SubmissionsDataByBrand()
    {
        // Arrange
        var brand = "Brand1";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brand);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults, Is.Not.Empty);
        Assert.Multiple(() =>
        {
            Assert.That(searchResults.TrueForAll(item => item.Brand.Contains(brand)), Is.True);
            Assert.That(searchResults[0].Submissions, Has.Count.EqualTo(2));
        });
        var submission1EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc);
        Assert.Multiple(() =>
        {
            Assert.That(submission1EntryTime.ToString(CultureInfo.InvariantCulture),
                Does.Contain(searchResults[0].Submissions[0].EntryTime));
            Assert.That(searchResults[0].Submissions[0].Price, Is.EqualTo(100));
            Assert.That(searchResults[0].Submissions[0].Description, Is.EqualTo("Description for Submission 1"));
        });
        var submission2EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc);
        Assert.Multiple(() =>
        {
            Assert.That(submission2EntryTime.ToString(CultureInfo.InvariantCulture),
                Does.Contain(searchResults[0].Submissions[1].EntryTime));
            Assert.That(searchResults[0].Submissions[1].Price, Is.EqualTo(180));
            Assert.That(searchResults[0].Submissions[1].Description, Is.EqualTo("Description for Submission 8"));
        });
    }


    /// <summary>
    ///     Tests that no results are returned when there are no submissions with the specified brand
    /// </summary>
    /// <author> Brandon Mora Umaña C15179 </author>
    [Test]
    public async Task GetSubmissionsByBrand_InvalidBrand_EmptyListReturned()
    {
        // Arrange
        var brand = "InvalidBrand";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brand);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Empty);
    }

    /// <summary>
    ///     Tests that no results are returned when the brand is empty
    /// </summary>
    /// <author> Brandon Mora Umaña C15179 </author>
    [Test]
    public async Task GetSubmissionsByBrand_EmptyBrand_EmptyListReturned()
    {
        // Arrange
        var brand = string.Empty;

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brand);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Empty);
    }

    /// <summary>
    ///     Checks if all items returned are within the range of price expected
    /// </summary>
    /// <author>Joseph Stuart Valverde Kong C18100 - Sprint 2</author>
    [Test]
    public async Task GetSubmissionsByPrice_ValidPrice_SubmissionsReturned()
    {
        // Arrange
        const long minPrice = 60;
        const long maxPrice = 200;
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByMinvalue, minPrice)
            .AddQueryParameter(SearchParameterTypes.SubmissionByMaxvalue, maxPrice);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.TrueForAll(item => item.Price is > minPrice and < maxPrice), Is.True);
    }

    /// <summary>
    ///     Searches for an item with a specific category and submissions are returned
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 - Sprint 2</author>
    [Test]
    public async Task SearchByCategory_ValidSearch_SubmissionReturned()
    {
        // Arrange
        var category = "Category1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByCategory, category);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(13));
        Assert.That(
            searchResults.TrueForAll(item => item.Categories.Any(i => i.Equals(category))),
            Is.True); // Verify that all items have the expected category name
    }

    /// <summary>
    ///     Searches for an item with a specific category and the result is empty
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 - Sprint 2</author>
    [Test]
    public async Task SearchByCategory_ValidSearch_EmptyResults()
    {
        // Arrange
        var category = "InvalidCategory";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByCategory, category);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0));
    }

    /// <summary>
    ///     Searches for an item with a specific category and the search is invalid
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 - Sprint 2</author>
    [Test]
    public async Task SearchByCategory_InvalidSearchEmptyString_EmptyResults()
    {
        // Arrange
        var category = string.Empty;

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByCategory, category)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCategory, category);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new ();
        
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0));
    }

    /// <summary>
    ///     Searches for an item with a specific userId and the search is empty
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 - SPRINT 3</author>
    [Test]
    public async Task SearchByUserId_InvalidSearchEmptyString_EmptyResults()
    {
        // Arrange
        var userIdToSearch = string.Empty;

        MockDataSetup();

        // Act
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByUserId, userIdToSearch);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);

        ItemMapper itemMapper = new();

        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0));
    }

    /// <summary>
    ///     Searches for an item with a specific invalid userId and the search is empty
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 - SPRINT 3 </author>
    [Test]
    public async Task SearchByInvalidUserId_SubmissionsFound()
    {
        // Arrange
        var userIdToSearch = "User333";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByUserId, userIdToSearch);

        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        ItemMapper itemMapper = new();
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0));
    }

    /// <summary>
    ///     Searches for an item with a specific invalid userId and the search is empty
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 - SPRINT 3 </author>
    [Test]
    public async Task SearchByUserId_SubmissionsFound()
    {
        // Arrange
        var userIdToSearch = "User1";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddQueryParameter(SearchParameterTypes.SubmissionByUserId, userIdToSearch);

        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        ItemMapper itemMapper = new();
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(1));
    }

    /// <summary>
    ///     Searching for an item with a specific user and brand, but no results
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 - SPRINT 3 </author>
    [Test]
    public async Task SearchByUserAndBrand_NoResults()
    {
        // Arrange
        var userIdToSearch = "User1";
        var brandToSearch = "NonExistentBrand";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByUserId, userIdToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brandToSearch);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        ItemMapper itemMapper = new();
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0));
    }


    /// <summary>
    ///     Searching for an item with a specific user and brand, but no results
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 - SPRINT 3 </author>
    [Test]
    public async Task SearchByPriceRangeAndBrand_ValidResults()
    {
        // Arrange
        var minPrice = 100;
        var maxPrice = 200;
        var brandToSearch = "Brand2";
        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByMinvalue, minPrice)
            .AddQueryParameter(SearchParameterTypes.SubmissionByMaxvalue, maxPrice)
            .AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brandToSearch);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        ItemMapper itemMapper = new();
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(3));
    }

    /// <summary>
    ///     Searching for an item with several different criteria to be certain that it holds several at once
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 - SPRINT 3 </author>
    [Test]
    public async Task SearchWithMultipleCriteria_ValidResults()
    {
        // Arrange
        var nameToSearch = "Product1";
        var provinceToSearch = "Province1";
        var cantonToSearch = "Canton1";
        var categoryToSearch = "Category1";
        var modelToSearch = "Model1";
        var brandToSearch = "Brand1";
        var userIdToSearch = "User1";

        MockDataSetup();
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByName, nameToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByProvince, provinceToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCanton, cantonToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCategory, categoryToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByModel, modelToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByBrand, brandToSearch)
            .AddQueryParameter(SearchParameterTypes.SubmissionByUserId, userIdToSearch);

        // Act
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        ItemMapper itemMapper = new();
        var searchResults = itemMapper.ToVm(searchResultsDto);

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.GreaterThan(0));
    }
    
    /// <summary>
    ///     Searches by location within a 5 km range
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SearchWithLocationWithinFiveKm()
    {
        MockDataSetup();
        
        MapVm mapVm = new MapVm(9.960912220799878, -84.08619109447427, 5);
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddUniqueSearch(submission => MapVm.Ratio * submission.Store.Location.Distance(mapVm.Location) <= mapVm.Distance,
            mapVmParam => mapVmParam.Location != null && mapVmParam.Distance != 0,
            mapVm);
        
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        ItemMapper itemMapper = new ();
        
        List<ItemVm> searchResults = itemMapper.ToVm(searchResultsDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(searchResults, Is.Not.Null);
            Assert.That(searchResults, Is.Not.Empty);
            Assert.That(searchResults, Has.Count.EqualTo(7));
        });
    }
    
    /// <summary>
    ///     Searches by location within a 4 km but no store is within that range
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SearchWithLocationWithinFourKmButIsFarther()
    {
        MockDataSetup();
        MapVm mapVm = new MapVm(9.960912220799878, -84.08619109447427, 4);
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddUniqueSearch(submission => MapVm.Ratio * submission.Store.Location.Distance(mapVm.Location) <= mapVm.Distance,
            mapVmParam => mapVmParam.Location != null && mapVmParam.Distance != 0,
            mapVm);
        
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        ItemMapper itemMapper = new ();
        
        List<ItemVm> searchResults = itemMapper.ToVm(searchResultsDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(searchResults, Is.Not.Null);
            Assert.That(searchResults, Is.Empty);
        });
    }
    
    /// <summary>
    ///     Searches by location on a 10 km range
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SearchWithLocationWithinTenKm()
    {
        MockDataSetup();
        MapVm mapVm = new MapVm(9.960912220799878, -84.08619109447427, 10);
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddUniqueSearch(submission => MapVm.Ratio * submission.Store.Location.Distance(mapVm.Location) <= mapVm.Distance,
            mapVmParam => mapVmParam.Location != null && mapVmParam.Distance != 0,
            mapVm);
        
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        ItemMapper itemMapper = new ();
        
        List<ItemVm> searchResults = itemMapper.ToVm(searchResultsDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(searchResults, Is.Not.Null);
            Assert.That(searchResults, Is.Not.Empty);
            Assert.That(searchResults, Has.Count.EqualTo(13));
        });
    }
    
    /// <summary>
    ///     Searches by location within a 20km range
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SearchWithLocationWithinTwentyKm()
    {
        MockDataSetup();
        MapVm mapVm = new MapVm(9.960912220799878, -84.08619109447427, 20);
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddUniqueSearch(submission => MapVm.Ratio * submission.Store.Location.Distance(mapVm.Location) <= mapVm.Distance,
            mapVmParam => mapVmParam.Location != null && mapVmParam.Distance != 0,
            mapVm);
        
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        ItemMapper itemMapper = new ();
        
        List<ItemVm> searchResults = itemMapper.ToVm(searchResultsDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(searchResults, Is.Not.Null);
            Assert.That(searchResults, Is.Not.Empty);
            Assert.That(searchResults, Has.Count.EqualTo(19));
        });
    }

    /// <summary>
    ///     Search by filter on location
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SearchByFilterWorks()
    {
        MockDataSetup();
        MapVm mapVm = new MapVm(9.960912220799878, -84.08619109447427, 20);
        
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria.AddFilterParameter(SearchParameterTypes.SubmissionByLocationFilter, mapVm);
        
        var searchResultsDto = await _searchService!.GetSearchSubmissionsAsync(searchCriteria);
        
        ItemMapper itemMapper = new ();
        
        List<ItemVm> searchResults = itemMapper.ToVm(searchResultsDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(searchResults, Is.Not.Null);
            Assert.That(searchResults, Is.Not.Empty);
            Assert.That(searchResults, Has.Count.EqualTo(19));
        });
    }

    /// <summary>
    ///     Search on simple query with default search method
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SimpleSearchByGetSearchResults()
    {
        MockDataSetup();
        
        SearchVm searchVm = new SearchVm()
        {
            ProductName = "Product1"
        };
        
        var searchResults = await _searchService!.GetSearchResultsAsync(searchVm);
        
        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResults);
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.Not.Empty);
            Assert.That(results, Has.Count.EqualTo(2));
        });
    }
    
    /// <summary>
    ///     Search on complex query on default search method
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task ComplexSearchByGetSearchResults()
    {
        MockDataSetup();
        
        SearchVm searchVm = new SearchVm()
        {
            ProductName = "Product1",
            ProvinceSelected  = "Province1",
            CantonSelected = "Canton1",
            CategorySelected = "Category1",
            ModelSelected = "Model1",
            BrandSelected = "Brand1"
        };
       
        var searchResults = await _searchService!.GetSearchResultsAsync(searchVm);
        
        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResults);
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.Not.Empty);
            Assert.That(results, Has.Count.EqualTo(1));
        });

        MockDataSetup();
    }

    /// <summary>
    ///     Searches by location on default search method
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task SearchLocationByGetSearchResults()
    {
        MockDataSetup();
        
        SearchVm searchVm = new SearchVm()
        {
            Latitude = 9.960912220799878, 
            Longitude = -84.08619109447427,
            Distance = 5
        };
        
        var searchResults = await _searchService!.GetSearchResultsAsync(searchVm);
        
        ItemMapper itemMapper = new ();
        
        var results = itemMapper.ToVm(searchResults);
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.Not.Empty);
            Assert.That(results, Has.Count.EqualTo(7));
        });
    }

    /// <summary>
    ///     Sets up the mock for the submission service so that it behaves as expected for the tests
    /// </summary>
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    private void MockDataSetup()
    {
        var country = new Country { Name = "Country" };

        var province1 = new Province { Name = "Province1", CountryName = "Country", Country = country };
        var province2 = new Province { Name = "Province2", CountryName = "Country", Country = country };

        var canton1 = new Canton { Name = "Canton1", ProvinceName = "Province1", Province = province1 };
        var canton2 = new Canton { Name = "Canton2", ProvinceName = "Province2", Province = province2 };

        var category1 = new Category { Name = "Category1" };
        var category2 = new Category { Name = "Category2" };
        var category3 = new Category { Name = "Category3" };

        // Add users
        var users = new List<User>
        {
            new()
            {
                Name = "User1"
            },
            new()
            {
                Name = "User2"
            },
            new()
            {
                Name = "User3"
            }
        };

        // Add stores
        var stores = new List<Store>
        {
            new()
            {
                Name = "Store1",
                Canton = canton1,
                Address = "Address1",
                Telephone = "Telephone1",
                Location = new Point(9.93801190922732, -84.05199796732124) { SRID = 4326 } // <5
            },
            new()
            {
                Name = "Store2",
                Canton = canton1,
                Address = "Address2",
                Telephone = "Telephone2",
                Location = new Point(9.936069142790782, -84.04010505878959) { SRID = 4326 } // > 5
            },
            new()
            {
                Name = "Store3",
                Canton = canton2,
                Address = "Address3",
                Telephone = "Telephone3",
                Location = new Point(9.924794131121706, -84.24072983843948) { SRID = 4326 } // > 10
            },
            new()
            {
                Name = "Store4",
                Canton = canton2,
                Address = "Address4",
                Telephone = "Telephone4",
                Location = new Point(10.05997217354618, -84.43718094445109) { SRID = 4326 } // > 20
            }
        };

        // Add products
        var products = new List<Product>
        {
            new()
            {
                Id = 1,
                Name = "Product1",
                Model = "Model1",
                Brand = "Brand1",
                Categories = new List<Category> { category1, category2 }
            },
            new()
            {
                Id = 2,
                Name = "Product2",
                Model = "Model2",
                Brand = "Brand2",
                Categories = new List<Category> { category2, category3 }
            },
            new()
            {
                Id = 3,
                Name = "Product3",
                Model = "Model3",
                Brand = "Brand3",
                Categories = new List<Category> { category1, category3 }
            },
            new()
            {
                Id = 4,
                Name = "Product4",
                Model = "Model4",
                Brand = "Brand4",
                Categories = new List<Category> { category1, category2 }
            },
            new()
            {
                Id = 5,
                Name = "Product5",
                Model = "Model5",
                Brand = "Brand5",
                Categories = new List<Category> { category2, category3 }
            },
            new()
            {
                Id = 6,
                Name = "Product6",
                Model = "Model6",
                Brand = "Brand6",
                Categories = new List<Category> { category1, category3 }
            },
            new()
            {
                Id = 7,
                Name = "Product7",
                Model = "Model7",
                Brand = "Brand7",
                Categories = new List<Category> { category1, category2 }
            }
        };

        // Add submissions
        var submissions = new List<Submission>
        {
            new()
            {
                UserId = "User1",
                EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
                Price = 100,
                Rating = 4.5f,
                Description = "Description for Submission 1",
                StoreName = "Store1",
                ProductId = 1,
                User = users[0],
                Store = stores[0],
                Product = products[0]
            },
            new()
            {
                UserId = "User2",
                EntryTime = DateTime.Now.AddDays(-1),
                Price = 200,
                Rating = 3.8f,
                Description = "Description for Submission 2",
                StoreName = "Store2",
                ProductId = 2,
                User = users[1],
                Store = stores[1],
                Product = products[1]
            },
            new()
            {
                UserId = "User3",
                EntryTime = DateTime.Now.AddDays(-3),
                Price = 50,
                Rating = 4.2f,
                Description = "Description for Submission 3",
                StoreName = "Store3",
                ProductId = 3,
                User = users[2],
                Store = stores[2],
                Product = products[2]
            },
            new()
            {
                UserId = "User4",
                EntryTime = DateTime.Now.AddDays(-4),
                Price = 150,
                Rating = 4.0f,
                Description = "Description for Submission 4",
                StoreName = "Store4",
                ProductId = 4,
                User = users[0],
                Store = stores[0],
                Product = products[3]
            },
            new()
            {
                UserId = "User5",
                EntryTime = DateTime.Now.AddDays(-5),
                Price = 75,
                Rating = 3.9f,
                Description = "Description for Submission 5",
                StoreName = "Store5",
                ProductId = 5,
                User = users[1],
                Store = stores[1],
                Product = products[4]
            },
            new()
            {
                UserId = "User6",
                EntryTime = DateTime.Now.AddDays(-6),
                Price = 220,
                Rating = 4.6f,
                Description = "Description for Submission 6",
                StoreName = "Store6",
                ProductId = 6,
                User = users[2],
                Store = stores[2],
                Product = products[5]
            },
            new()
            {
                UserId = "User7",
                EntryTime = DateTime.Now.AddDays(-7),
                Price = 90,
                Rating = 3.7f,
                Description = "Description for Submission 7",
                StoreName = "Store7",
                ProductId = 7,
                User = users[0],
                Store = stores[0],
                Product = products[6]
            },
            new()
            {
                UserId = "User8",
                EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
                Price = 180,
                Rating = 4.3f,
                Description = "Description for Submission 8",
                StoreName = "Store1",
                ProductId = 1,
                User = users[0],
                Store = stores[0],
                Product = products[0]
            },
            new()
            {
                UserId = "User9",
                EntryTime = DateTime.Now.AddDays(-9),
                Price = 120,
                Rating = 4.1f,
                Description = "Description for Submission 9",
                StoreName = "Store9",
                ProductId = 9,
                User = users[2],
                Store = stores[2],
                Product = products[1]
            },
            new()
            {
                UserId = "User10",
                EntryTime = DateTime.Now.AddDays(-10),
                Price = 70,
                Rating = 3.5f,
                Description = "Description for Submission 10",
                StoreName = "Store10",
                ProductId = 10,
                User = users[0],
                Store = stores[0],
                Product = products[2]
            },
            new()
            {
                UserId = "User11",
                EntryTime = DateTime.Now.AddDays(-11),
                Price = 110,
                Rating = 4.4f,
                Description = "Description for Submission 11",
                StoreName = "Store11",
                ProductId = 11,
                User = users[1],
                Store = stores[1],
                Product = products[3]
            },
            new()
            {
                UserId = "User12",
                EntryTime = DateTime.Now.AddDays(-12),
                Price = 240,
                Rating = 4.8f,
                Description = "Description for Submission 12",
                StoreName = "Store12",
                ProductId = 12,
                User = users[2],
                Store = stores[2],
                Product = products[4]
            },
            new()
            {
                UserId = "User13",
                EntryTime = DateTime.Now.AddDays(-13),
                Price = 85,
                Rating = 3.6f,
                Description = "Description for Submission 13",
                StoreName = "Store13",
                ProductId = 13,
                User = users[0],
                Store = stores[0],
                Product = products[5]
            },
            new()
            {
                UserId = "User14",
                EntryTime = DateTime.Now.AddDays(-14),
                Price = 130,
                Rating = 4.0f,
                Description = "Description for Submission 14",
                StoreName = "Store14",
                ProductId = 14,
                User = users[1],
                Store = stores[1],
                Product = products[6]
            },
            new()
            {
                UserId = "User15",
                EntryTime = DateTime.Now.AddDays(-15),
                Price = 190,
                Rating = 4.2f,
                Description = "Description for Submission 15",
                StoreName = "Store2",
                ProductId = 1,
                User = users[2],
                Store = stores[2],
                Product = products[0]
            },
            new()
            {
                UserId = "User16",
                EntryTime = DateTime.Now.AddDays(-16),
                Price = 65,
                Rating = 3.4f,
                Description = "Description for Submission 16",
                StoreName = "Store16",
                ProductId = 16,
                User = users[0],
                Store = stores[0],
                Product = products[1]
            },
            new()
            {
                UserId = "User17",
                EntryTime = DateTime.Now.AddDays(-17),
                Price = 160,
                Rating = 4.1f,
                Description = "Description for Submission 17",
                StoreName = "Store17",
                ProductId = 17,
                User = users[1],
                Store = stores[1],
                Product = products[2]
            },
            new()
            {
                UserId = "User18",
                EntryTime = DateTime.Now.AddDays(-18),
                Price = 210,
                Rating = 4.6f,
                Description = "Description for Submission 18",
                StoreName = "Store18",
                ProductId = 18,
                User = users[2],
                Store = stores[2],
                Product = products[3]
            },
            new()
            {
                UserId = "User19",
                EntryTime = DateTime.Now.AddDays(-19),
                Price = 80,
                Rating = 3.7f,
                Description = "Description for Submission 19",
                StoreName = "Store19",
                ProductId = 19,
                User = users[0],
                Store = stores[0],
                Product = products[4]
            },
            new()
            {
                UserId = "User20",
                EntryTime = DateTime.Now.AddDays(-20),
                Price = 140,
                Rating = 3.9f,
                Description = "Description for Submission 20",
                StoreName = "Store20",
                ProductId = 20,
                User = users[1],
                Store = stores[1],
                Product = products[5]
            }
        };

        // setting up mock repository behavior requires the methods to be virtual on class being mocked or using interface, in this case
        // the methods are virtual because interface does not have the methods being implemented.
        _submissionRepositoryMock!
            .Setup(repo => repo.GetByDynamicQuery(It.IsAny<ISearchQueries<Submission>>()))
            .ReturnsAsync((ISearchQueries<Submission> searchQueries) =>
            {
                // initiate the query
                IQueryable<Submission>? submissionsResults =
                    submissions.AsQueryable().Include(submission => submission.Product);
                
                // append the search queries to the query
                submissionsResults = searchQueries.ApplySearch(submissionsResults);
                submissionsResults = searchQueries.ApplyUniqueSearches(submissionsResults);

                if (submissionsResults == null) return new List<Submission>();

                var results = searchQueries.ApplySearchFilters(submissionsResults);

                // get and return the results
                return results;
            });
    }
}