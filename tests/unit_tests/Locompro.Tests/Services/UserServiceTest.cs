using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Results;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

/// <summary>
///     Contains unit tests for UserService class.
///     Author: Brandon Alonso Mora Umaña.
/// </summary>
[TestFixture]
public class UserServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<ILoggerFactory> _mockLoggerFactory;
    private UserService _userService;
    private Mock<ISubmissionRepository> _mockSubmissionRepository;

    [SetUp]
    public void SetUp()
    {
        // Create mock instances
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockSubmissionRepository = new Mock<ISubmissionRepository>();
        // Setup mock behavior
        _mockUnitOfWork.Setup(uow => uow.GetSpecialRepository<IUserRepository>()).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetSpecialRepository<ISubmissionRepository>())
            .Returns(_mockSubmissionRepository.Object);
        // Instantiate the service with mocked dependencies
        _userService = new UserService(_mockUnitOfWork.Object, _mockLoggerFactory.Object);
    }

    /// <summary>
    ///     Test to ensure GetQualifiedUserIDs returns expected user IDs.
    ///     Author: Brandon Alonso Mora Umaña - Sprint 2
    /// </summary>
    [Test]
    public void GetQualifiedUserIDs_ReturnsExpectedUserIDs()
    {
        // Arrange
        var expectedUsers = new List<GetQualifiedUserIDsResult>
        {
            new() { Id = "user1" },
            new() { Id = "user2" }
        };
        _mockUserRepository.Setup(repo => repo.GetQualifiedUserIDs()).Returns(expectedUsers);

        // Act
        var result = _userService.GetQualifiedUserIDs();

        // Assert
        Assert.That(result, Is.EqualTo(expectedUsers));
    }

    /// <summary>
    ///     Test method to verify if the correct count of submissions by a user is returned.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetSubmissionsCountByUser_ReturnsExpectedCount()
    {
        // Arrange
        var expectedCount = 5; // replace with the expected count
        _mockUserRepository.Setup(repo => repo.GetSubmissionsCountByUser(It.IsAny<string>())).Returns(expectedCount);

        // Act
        var result = _userService.GetSubmissionsCountByUser("testUser");

        // Assert
        Assert.That(result, Is.EqualTo(expectedCount));
    }

    /// <summary>
    ///     Test method to verify if the correct count of reported submissions by a user is returned.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetReportedSubmissionsCountByUser_ReturnsExpectedCount()
    {
        // Arrange
        var expectedCount = 5; // replace with the expected count
        _mockUserRepository.Setup(repo => repo.GetReportedSubmissionsCountByUser(It.IsAny<string>()))
            .Returns(expectedCount);

        // Act
        var result = _userService.GetReportedSubmissionsCountByUser("testUser");

        // Assert
        Assert.That(result, Is.EqualTo(expectedCount));
    }

    /// <summary>
    ///     Test method to verify if the correct count of rated submissions by a user is returned.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetRatedSubmissionsCountByUser()
    {
        // Arrange
        var expectedCount = 5; // replace with the expected count
        _mockUserRepository.Setup(repo => repo.GetRatedSubmissionsCountByUser(It.IsAny<string>()))
            .Returns(expectedCount);

        // Act
        var result = _userService.GetRatedSubmissionsCountByUser("testUser");

        // Assert
        Assert.That(result, Is.EqualTo(expectedCount));
    }

    /// <summary>
    ///     Generates a list of test user information for testing purposes.
    /// </summary>
    /// <returns>A list of MostReportedUsersResult.</returns>
    public List<MostReportedUsersResult> GetTestList()
    {
        List<MostReportedUsersResult> testList = new();
        for (var i = 0; i < 12; i++)
            testList.Add(GenerateRandomTestUserInfo());
        return testList;
    }

    /// <summary>
    ///     Generates a random user information instance for testing.
    /// </summary>
    /// <returns>A MostReportedUsersResult instance with randomly generated data.</returns>
    public MostReportedUsersResult GenerateRandomTestUserInfo()
    {
        var testNumber = new Random().Next(0, 10000000);

        var userInfo = new MostReportedUsersResult
        {
            UserRating = new Random().NextSingle() * 5,
            UserName = $"test user{testNumber}",
            ReportedSubmissionCount = testNumber,
            TotalUserSubmissions = testNumber + 10
        };
        return userInfo;
    }

    /// <summary>
    ///     Test method to verify if the method returns the expected size of the list of most reported users.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetMostReportedUser_ReturnsExpectedListSize()
    {
        // Arrange
        var testListOfMostReportedUser = GetTestList();
        _mockUserRepository.Setup(repo => repo.GetMostReportedUsersInfo()).Returns(testListOfMostReportedUser);
        // Act
        var results = _userService.GetMostReportedUsersInfo();
        // Assert 
        Assert.That(results, Has.Count.LessThan(testListOfMostReportedUser.Count));
        Assert.That(results, Has.Count.EqualTo(10));
    }

    /// <summary>
    ///     Test method to verify if the list of most reported users is returned in the correct order.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetMostReportedUser_ReturnsOrderList()
    {
        // Arrange
        var testListOfMostReportedUser = GetTestList();
        _mockUserRepository.Setup(repo => repo.GetMostReportedUsersInfo()).Returns(testListOfMostReportedUser);
        // Act
        var results = _userService.GetMostReportedUsersInfo();
        // Assert 
        Assert.That(results.Count, Is.LessThan(testListOfMostReportedUser.Count));
        for (var i = 0; i < results.Count - 1; i++)
            Assert.That(results[i].ReportedSubmissionCount, Is.GreaterThan(results[i + 1].ReportedSubmissionCount));
    }

    [Test]
    public async Task GetShoppingList_ReturnsExpectedShoppingList()
    {
        // Arrange
        var user = GenerateTestUser();
        var expectedProducts = GenerateTestProducts(user);
        var userId = user.Id;
        user.ShoppedProducts = expectedProducts;

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetShoppingList(userId);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Products, Has.Count.EqualTo(expectedProducts.Count));
        });
        foreach (var productDto in result.Products)
        {
            var product = expectedProducts.Find(p => p.Id == productDto.Id);
            Assert.Multiple(() =>
            {
                Assert.That(productDto.Name, Is.EqualTo(product.Name));
                Assert.That(productDto.Model, Is.EqualTo(product.Model));
                Assert.That(productDto.Brand, Is.EqualTo(product.Brand));
                Assert.That(productDto.MinPrice, Is.EqualTo(product.Submissions.Min(s => s.Price)));
                Assert.That(productDto.MaxPrice, Is.EqualTo(product.Submissions.Max(s => s.Price)));
                Assert.That(productDto.TotalSubmissions, Is.EqualTo(product.Submissions.Count));
            });
        }
    }

    [Test]
    public async Task GetShoppingListSummary_ReturnsExpectedSummary()
    {
        // Arrange
        var user = GenerateTestUser();
        var expectedProducts = GenerateTestProducts(user);
        var userId = user.Id;
        user.ShoppedProducts = expectedProducts;
        
        var productSummaryStoresExpected = GenerateTestProductSummaryStores();
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
        
        var expectedSummary = GenerateTestShoppingListSummaryStoreDto();
       _mockSubmissionRepository.Setup(repo => repo.GetProductSummaryByStore(It.IsAny<List<int>>()))
                .ReturnsAsync(productSummaryStoresExpected);
       
        var expectedResult = new ShoppingListSummaryDto
        {
            UserId = userId,
            Stores = expectedSummary
        };
        
        // Act
        var result = await _userService.GetShoppingListSummary(userId);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Stores, Has.Count.EqualTo(expectedSummary.Count));
        });
        foreach (var storeDto in result.Stores)
        {
            var store = productSummaryStoresExpected.Find(s => s.Name == storeDto.Name);
            Assert.Multiple(() =>
            {
                Assert.That(storeDto.Name, Is.EqualTo(store.Name));
                Assert.That(storeDto.Province, Is.EqualTo(store.Province.Name));
                Assert.That(storeDto.Canton, Is.EqualTo(store.Canton.Name));
                Assert.That(storeDto.ProductsAvailable, Is.EqualTo(store.ProductsAvailable));
                Assert.That(storeDto.PercentageProductsAvailable, Is.EqualTo(store.PercentageProductsAvailable));
                Assert.That(storeDto.TotalCost, Is.EqualTo(store.TotalCost));
            });
        }
    }

    private static User GenerateTestUser()
    {
        var random = new Random();
        var name = $"User{random.Next(1, 100)}";
        var user = new User
        {
            Id = name,
            UserName = name,
            Name = name,
            Address = $"Address{random.Next(1, 100)}",
            Rating = (float)random.NextDouble() * 5,
            Status = Status.Active,
            CreatedSubmissions = new List<Submission>(),
            ApprovedSubmissions = new List<Submission>(),
            RejectedSubmissions = new List<Submission>(),
            ShoppedProducts = new List<Product>()
        };

        return user;
    }

    private List<Product> GenerateTestProducts(User user)
    {
        var testProducts = new List<Product>();
        var random = new Random();
        var userList = new List<User> { user };

        for (var i = 0; i < 3; i++)
        {
            var product = new Product
            {
                Id = i,
                Name = $"Product {i}",
                Model = $"Model {i}",
                Brand = $"Brand {i}",
                Status = Status.Active,
                Categories = new List<Category>(),
                Submissions = new List<Submission>(),
                Shoppers = userList
            };
            product.Submissions = GenerateTestSubmissions(product, user);
            testProducts.Add(product);
        }

        return testProducts;
    }

    private List<Submission> GenerateTestSubmissions(Product product, User user)
    {
        var testSubmissions = new List<Submission>();
        var random = new Random();
        var userList = new List<User>();

        for (var i = 0; i < 3; i++)
        {
            var submission = new Submission
            {
                UserId = user.Id,
                EntryTime = DateTime.Now,
                Status = SubmissionStatus.New,
                Price = random.Next(1000, 5000),
                Rating = (float)random.NextDouble() * 5,
                Description = $"Description {i}",
                StoreName = $"Store {i}",
                ProductId = product.Id,
                User = user,
                Store = new Store(),
                Product = product,
                Pictures = new List<Picture>(),
                UserReports = new List<UserReport>(),
                AutoReports = new List<AutoReport>(),
                NumberOfRatings = random.Next(1, 100),
                Approvers = userList,
                Rejecters = userList
            };
            testSubmissions.Add(submission);
        }

        return testSubmissions;
    }

    private List<ProductSummaryStore> GenerateTestProductSummaryStores()
    {
        var testList = new List<ProductSummaryStore>();
        var defaultCanton = CreateDefaultCanton();
        for (var i = 0; i < 2; i++)
        {
            var store = new ProductSummaryStore
            {
                Name = $"Store {i}",
                Province = defaultCanton.Province,
                Canton = defaultCanton,
                ProductsAvailable = i,
                PercentageProductsAvailable = i,
                TotalCost = 5000 + i
            };

            testList.Add(store);
        }

        return testList;
    }

    private List<ShoppingListSummaryStoreDto> GenerateTestShoppingListSummaryStoreDto()
    {
        var productSummaryStores = GenerateTestProductSummaryStores();
        return productSummaryStores.Select(productSummaryStore => new ShoppingListSummaryStoreDto
            {
                Name = productSummaryStore.Name,
                Province = productSummaryStore.Province.Name,
                Canton = productSummaryStore.Canton.Name,
                ProductsAvailable = productSummaryStore.ProductsAvailable,
                PercentageProductsAvailable = productSummaryStore.PercentageProductsAvailable,
                TotalCost = productSummaryStore.TotalCost
            })
            .ToList();
    }

    private Country CreateDefaultCountry()
    {
        var country = new Country
        {
            Name = "Costa Rica",
            Provinces = new List<Province>()
        };

        return country;
    }

    private Province CreateDefaultProvince()
    {
        var province = new Province
        {
            Name = "Alajuela",
            CountryName = "Costa Rica",
            Cantons = new List<Canton>(),
            Country = CreateDefaultCountry()
        };
        province.Country.Provinces.Add(province);

        return province;
    }

    private Canton CreateDefaultCanton()
    {
        var canton = new Canton
        {
            Name = "Alajuela",
            ProvinceName = "Alajuela",
            CountryName = "Costa Rica",
            Province = CreateDefaultProvince()
        };
        canton.Province.Cantons.Add(canton);

        return canton;
    }
}