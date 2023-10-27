using System.Globalization;
using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.ContentModel;

namespace Locompro.Tests.Services;

[TestFixture]
public class SearchServiceTest
{
    private Mock<LocomproContext> _dbContextMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILoggerFactory> _loggerFactoryMock;
    private Mock<SubmissionRepository> _submissionCrudRepositoryMock;
    private Mock<ISubmissionService> _submissionServiceMock;
    private SearchService _searchService;
    [SetUp]
    public void Setup()
    {
        _dbContextMock = new Mock<LocomproContext>(new DbContextOptions<LocomproContext>());
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _submissionCrudRepositoryMock =
            new Mock<SubmissionRepository>(_dbContextMock.Object, _loggerFactoryMock.Object);
        _submissionServiceMock = new Mock<ISubmissionService>();
        _searchService = new SearchService(_submissionServiceMock.Object);
    }
    
    /// <summary>
    /// Finds a list of names that are expected to be found
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void SearchByName_NameIsFound()
    {
        // Arrange
        string productSearchName = "Product1";
        MockDataSetup();
        List<Item> searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));

        productSearchName = "Product2";
        searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));

        productSearchName = "Product3";
        searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));

        productSearchName = "Product4";
        searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));

        productSearchName = "Product5";
        searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));

        productSearchName = "Product6";
        searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));

        productSearchName = "Product7";
        searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.IsTrue(searchResults.Exists(i => i.Name == productSearchName));
    }

    /// <summary>
    /// Searches for name that is not expected to be found and
    /// returns empty list
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void SearchByNameForNonExistent_NameIsNotFound()
    {
        // Arrange
        string productSearchName = "ProductNonExistent";
        MockDataSetup();
        List<Item> searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsFalse(searchResults.Exists(i => i.Name == productSearchName));
            Assert.IsEmpty(searchResults);
        });
    }

    /// <summary>
    /// Gives an empty string and returns empty list
    /// Should not throw an exception
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void SearchByNameForEmptyString_NameIsNotFound()
    {
        // Arrange
        string productSearchName = "";
        MockDataSetup();
        List<Item> searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsFalse(searchResults.Exists(i => i.Name == productSearchName));
            Assert.IsEmpty(searchResults);
        });
    }

    /// <summary>
    /// Searches for an item and the result is the one expected
    /// to be the best submission
    /// <remarks>
    /// This test is to be changed accordingly when
    /// a new heuristic change to the best submission algorithm is made
    /// </remarks>
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void SearchByName_FindsBestSubmission()
    {
        string productSearchName = "Product1";
        MockDataSetup();
        List<Item> searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;

        DateTime dateTimeExpected = new DateTime(2023, 10, 6, 0, 0, 0, DateTimeKind.Utc);
        DateTime dateTimeReceived = DateTime.Parse(searchResults[0].LastSubmissionDate, new CultureInfo("en-US"));

        // Assert
        Assert.That(dateTimeReceived, Is.EqualTo(dateTimeExpected));
    }
     
    /// <summary>
    /// Tests that the number of submissions given by the search are correct
    /// <author>Gabriel Molina Bulgarelli C14826</author>
    /// </summary>
    [Test]
    public void AmountOfSearchSubmissions()
    {
        string productSearchName = "Product1";
        MockDataSetup();
        var searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;
        
        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults.Count, Is.GreaterThan(0));
        Assert.That(searchResults[0].Submissions.Count, Is.EqualTo(2));
    }
    
    /// <summary>
    /// Tests that the data given on submissions given by a search are correctly assigned
    /// <author>Gabriel Molina Bulgarelli C14826</author>
    /// </summary>
    [Test]
    public void DataOfSearchSubmissions()
    {
        string productSearchName = "Product1";
        MockDataSetup();
        var searchResults = _searchService.SearchItems(productSearchName, null, null, 0, 0, null, null).Result;
        
        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults.Count, Is.GreaterThan(0));
        Assert.That(searchResults[0].Submissions.Count, Is.EqualTo(2));
        
        Assert.That(searchResults[0].Submissions[0].UserId, Is.EqualTo("User1"));
        DateTime submission1EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(searchResults[0].Submissions[0].EntryTime, Is.EqualTo(submission1EntryTime));
        Assert.That(searchResults[0].Submissions[0].Price, Is.EqualTo(100));
        Assert.That(searchResults[0].Submissions[0].Description, Is.EqualTo("Description for Submission 1"));
        Assert.That(searchResults[0].Submissions[0].StoreName, Is.EqualTo("Store1"));
        Assert.That(searchResults[0].Submissions[0].ProductId, Is.EqualTo(1));

        Assert.That(searchResults[0].Submissions[1].UserId, Is.EqualTo("User8"));
        DateTime submission2EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(searchResults[0].Submissions[1].EntryTime, Is.EqualTo(submission2EntryTime));
        Assert.That(searchResults[0].Submissions[1].Price, Is.EqualTo(180));
        Assert.That(searchResults[0].Submissions[1].Description, Is.EqualTo("Description for Submission 8"));
        Assert.That(searchResults[0].Submissions[1].StoreName, Is.EqualTo("Store1"));
        Assert.That(searchResults[0].Submissions[1].ProductId, Is.EqualTo(1));
    }
    
    /// <summary>
    /// Searches for an item with a specific model and the result is the one expected
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 </author>
    [Test]
    public void SearchByModel_ModelIsFound()
    {
        // Arrange
        string modelName = "Model1";
        MockDataSetup();
        // Act
        List<Item> searchResults = _searchService.SearchItems(null, null, null, 0, 0, null, modelName).Result;

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults.Count > 0, Is.True);
        Assert.That(searchResults.TrueForAll(item => item.Submissions[0].Product.Model.Contains(modelName)),
            Is.True); // Verify that all items have the expected model name
    }

    /// <summary>
    /// Tests that the correct submissions result data is returned when the model is specified in search
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826 </author>
    [Test]
    public void SubmissionsDataByModel()
    {
        // Arrange
        string modelName = "Model2";
        MockDataSetup();
        // Act
        List<Item> searchResults = _searchService.SearchItems(null, null, null, 0, 0, null, modelName).Result;

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults.Count, Is.GreaterThan(0));
        Assert.That(searchResults.TrueForAll(item => item.Submissions[0].Product.Model.Contains(modelName)),
            Is.True);
        Assert.That(searchResults[0].Submissions.Count, Is.EqualTo(1));

        Assert.That(searchResults[0].Submissions[0].UserId, Is.EqualTo("User2"));
        Assert.That(searchResults[0].Submissions[0].Price, Is.EqualTo(200));
        Assert.That(searchResults[0].Submissions[0].Description, Is.EqualTo("Description for Submission 2"));
        Assert.That(searchResults[0].Submissions[0].StoreName, Is.EqualTo("Store2"));
        Assert.That(searchResults[0].Submissions[0].ProductId, Is.EqualTo(2));
    }
    
    /// <summary>
    ///  Searches for an item with a specific model and the result is empty
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 </author>
    [Test]
    public void SearchByModel_ModelIsNotFound()
    {
        // Arrange
        string modelName = "NonExistentModel";
        MockDataSetup();
        // Act
        List<Item> searchResults = _searchService.SearchItems(null, null, null, 0, 0, null, modelName).Result;

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0)); // Expecting an empty result
    }

    /// <summary>
    /// Searches for an empty model and the result is empty, according to the expected behavior
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña C15179 </author>
    [Test]
    public void SearchByModel_EmptyModelName()
    {
        // Arrange
        string modelName = string.Empty;
        MockDataSetup();
        // Act
        List<Item> searchResults = _searchService.SearchItems(null, null, null, 0, 0, null, modelName).Result;

        // Assert
        Assert.IsNotNull(searchResults);
        Assert.That(searchResults.Count, Is.EqualTo(0)); // Expecting an empty result
    }

    /// <summary>
    /// Tests that the correct result is returned when the brand is specified in search
    /// </summary>
    /// <author> Brandon Mora Umaña C15179 </author>
    [Test]
    public async Task GetSubmissionsByBrand_ValidBrand_SubmissionsReturned()
    {
        // Arrange
        var brand = "Brand1";
        MockDataSetup();
        // Act
        var results = await _searchService.SearchItems(null, null, null, 0, 0, null, null, brand);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.GreaterThan(0));
        Assert.That(results.TrueForAll(item => item.Submissions[0].Product.Brand.Contains(brand)), Is.True);
    }
    
    /// <summary>
    /// Tests that the correct submissions result data is returned when the brand is specified in search
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli C14826</author>
    [Test]
    public async Task SubmissionsDataByBrand()
    {
        // Arrange
        var brand = "Brand1";
        MockDataSetup();
        // Act
        var searchResults = await _searchService.SearchItems(null, null, null, 0, 0, null, null, brand);

        // Assert
        Assert.That(searchResults, Is.Not.Null);
        Assert.That(searchResults.Count, Is.GreaterThan(0));
        Assert.That(searchResults.TrueForAll(item => item.Submissions[0].Product.Brand.Contains(brand)), Is.True);
        Assert.That(searchResults[0].Submissions.Count, Is.EqualTo(2));

        Assert.That(searchResults[0].Submissions[0].UserId, Is.EqualTo("User1"));
        DateTime submission1EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(searchResults[0].Submissions[0].EntryTime, Is.EqualTo(submission1EntryTime));
        Assert.That(searchResults[0].Submissions[0].Price, Is.EqualTo(100));
        Assert.That(searchResults[0].Submissions[0].Description, Is.EqualTo("Description for Submission 1"));
        Assert.That(searchResults[0].Submissions[0].StoreName, Is.EqualTo("Store1"));
        Assert.That(searchResults[0].Submissions[0].ProductId, Is.EqualTo(1));

        Assert.That(searchResults[0].Submissions[1].UserId, Is.EqualTo("User8"));
        DateTime submission2EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(searchResults[0].Submissions[1].EntryTime, Is.EqualTo(submission2EntryTime));
        Assert.That(searchResults[0].Submissions[1].Price, Is.EqualTo(180));
        Assert.That(searchResults[0].Submissions[1].Description, Is.EqualTo("Description for Submission 8"));
        Assert.That(searchResults[0].Submissions[1].StoreName, Is.EqualTo("Store1"));
        Assert.That(searchResults[0].Submissions[1].ProductId, Is.EqualTo(1));
    }
    
    
    /// <summary>
    /// Tests that no results are returned when there are no submissions with the specified brand
    /// </summary>
    /// <author> Brandon Mora Umaña C15179 </author>
    [Test]
    public async Task GetSubmissionsByBrand_InvalidBrand_EmptyListReturned()
    {
        // Arrange
        string brand = "InvalidBrand";
        MockDataSetup();
        // Act
        var results = await _searchService.SearchItems(null, null, null, 0, 0, null, null, brand);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that no results are returned when the brand is empty
    /// </summary>
    /// <author> Brandon Mora Umaña C15179 </author>
    [Test]
    public async Task GetSubmissionsByBrand_EmptyBrand_EmptyListReturned()
    {
        // Arrange
        string brand = string.Empty;
        MockDataSetup();
        // Act
        var results = await _searchService.SearchItems(null, null, null, 0, 0, null, null, brand);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(0));
    }


    /// <summary>
    /// Sets up the mock for the submission service so that it behaves as expected for the tests
    /// </summary>
    void MockDataSetup()
    {
        Country country = new Country { Name = "Country" };

        Province province1 = new Province { Name = "Province1", CountryName = "Country", Country = country };
        Province province2 = new Province { Name = "Province2", CountryName = "Country", Country = country };

        Canton canton1 = new Canton { Name = "Canton1", ProvinceName = "Province1", Province = province1 };
        Canton canton2 = new Canton { Name = "Canton2", ProvinceName = "Province2", Province = province2 };

        // Add users
        List<User> users = new List<User>
        {
            new User
            {
                Name = "User1"
            },
            new User
            {
                Name = "User2"
            },
            new User
            {
                Name = "User3"
            }
        };

        // Add stores
        List<Store> stores = new List<Store>
        {
            new Store
            {
                Name = "Store1",
                Canton = canton1,
                Address = "Address1",
                Telephone = "Telephone1",
            },
            new Store
            {
                Name = "Store2",
                Canton = canton1,
                Address = "Address2",
                Telephone = "Telephone2",
            },
            new Store
            {
                Name = "Store3",
                Canton = canton2,
                Address = "Address3",
                Telephone = "Telephone3",
            },
            new Store
            {
                Name = "Store4",
                Canton = canton2,
                Address = "Address4",
                Telephone = "Telephone4",
            }
        };

        // Add products
        List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Product1",
                Model = "Model1",
                Brand = "Brand1"
            },
            new Product
            {
                Id = 2,
                Name = "Product2",
                Model = "Model2",
                Brand = "Brand2"
            },
            new Product
            {
                Id = 3,
                Name = "Product3",
                Model = "Model3",
                Brand = "Brand3"
            },
            new Product
            {
                Id = 4,
                Name = "Product4",
                Model = "Model4",
                Brand = "Brand4"
            },
            new Product
            {
                Id = 5,
                Name = "Product5",
                Model = "Model5",
                Brand = "Brand5"
            },
            new Product
            {
                Id = 6,
                Name = "Product6",
                Model = "Model6",
                Brand = "Brand6"
            },
            new Product
            {
                Id = 7,
                Name = "Product7",
                Model = "Model7",
                Brand = "Brand7"
            }
        };

        // Add submissions
        List<Submission> submissions = new List<Submission>
        {
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
            new Submission
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
        _submissionServiceMock
            .Setup(service => service.GetByCanton(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string canton, string province) =>
            {
                return submissions
                    .Where(s => s.Store.Canton.Name == canton && s.Store.Canton.ProvinceName == province).ToList();
            });

        _submissionServiceMock.Setup(service => service.GetByProductModel(It.IsAny<string>()))
            .ReturnsAsync((string model) => { return submissions.Where(s => s.Product.Model == model).ToList(); });

        _submissionServiceMock.Setup(service => service.GetByProductName(It.IsAny<string>()))
            .ReturnsAsync((string productName) =>
            {
                return submissions.Where(s => s.Product.Name == productName).ToList();
            });
        _submissionServiceMock.Setup(service => service.GetByBrand(It.IsAny<string>()))
            .ReturnsAsync((string brand) => { return submissions.Where(s => s.Product.Brand == brand).ToList(); });
    }
}