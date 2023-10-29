using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class SubmissionServiceTest
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ISubmissionRepository> _submissionCrudRepositoryMock;
    private SubmissionService _submissionService;
    
    [SetUp]
    public void Setup()
    {
        var loggerFactoryMock = new Mock<ILoggerFactory>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _submissionCrudRepositoryMock = new Mock<ISubmissionRepository>();
        
        _unitOfWorkMock
            .Setup(unit => unit.GetRepository<ISubmissionRepository>())
            .Returns(_submissionCrudRepositoryMock.Object);
        
        _submissionService = new SubmissionService(_unitOfWorkMock.Object, loggerFactoryMock.Object);
    }
    
    /// <summary>
    ///   tests that the search by canton and province returns the expected results when the canton and province are mentioned in the submissions
    /// <author> A. Badilla Olivas B80874 </author>
    /// </summary>
    [Test]
    public async Task GetSubmissionsByCantonAndProvince_ValidCantonAndProvince_SubmissionsReturned()
    {
        // Arrange
        string canton = "Canton1";
        string province = "Province1";
        MockDataSetup();
        // Act
        var results = await _submissionService.GetByCantonAndProvince(canton, province);

        // Assert
        var submissions = results as Submission[] ?? results.ToArray();
        Assert.That(submissions, Is.Not.Null);
        Assert.That(submissions.Count(), Is.GreaterThan(0));
        bool all = true;
        foreach (var sub in submissions)
        {
            if (sub.Store.Canton.Name != canton || sub.Store.Canton.ProvinceName != province)
            {
                all = false;
                break;
            }
        }

        Assert.That(all, Is.True);
    }

    /// <summary>
    /// Tests that an empty list is returned when the canton and province are not mentioned in any submission
    /// <author> A. Badilla Olivas B80874 </author>
    /// </summary>
    [Test]
    public async Task GetSubmissionsByCantonAndProvince_InvalidCantonAndProvince_EmptyListReturned()
    {
        // Arrange
        string canton = "InvalidCanton";
        string province = "InvalidProvince";
        MockDataSetup();
        // Act
        var results = await _submissionService.GetByCantonAndProvince(canton, province);

        // Assert
        var submissions = results as Submission[] ?? results.ToArray();
        Assert.That(submissions, Is.Not.Null);
        Assert.That(submissions.Count(), Is.EqualTo(0));
    }

    /// <summary>
    /// Sets up the mock for the submission repository so that it behaves as expected for the tests
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
        
        _submissionCrudRepositoryMock
            .Setup(repo => repo.GetByCantonAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string canton, string province) =>
            {
                return submissions
                    .Where(s => s.Store.Canton.Name == canton && s.Store.Canton.ProvinceName == province).ToList();
            });

        _submissionCrudRepositoryMock.Setup(repo => repo.GetByProductModelAsync(It.IsAny<string>()))
            .ReturnsAsync((string model) => { return submissions.Where(s => s.Product.Model == model).ToList(); });

        _submissionCrudRepositoryMock.Setup(repo => repo.GetByProductNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string productName) =>
            {
                return submissions.Where(s => s.Product.Name == productName).ToList();
            });
        _submissionCrudRepositoryMock.Setup(repo => repo.GetByBrandAsync(It.IsAny<string>()))
            .ReturnsAsync((string brand) => { return submissions.Where(s => s.Product.Brand == brand).ToList(); });
    }
}