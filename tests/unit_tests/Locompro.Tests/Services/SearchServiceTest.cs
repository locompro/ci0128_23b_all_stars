using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Locompro.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

public class SearchServiceTest
{
    private Mock<ILoggerFactory> _logger;
    
    // Db sets for context
    private Mock<DbSet<Submission>> _submissionDbSet;
    private Mock<DbSet<Product>> _productDbSet;
    private Mock<DbSet<User>> _userDbSet;
    private Mock<DbSet<Store>> _storeDbSet;
    
    // context
    private Mock<LocomproContext> _context;
    
    // repository
    private SubmissionRepository _submissionRepository;
    
    // search service
    private SearchService _searchService;
    
    [SetUp]
    public void SetUp()
    {
        DbContextOptions<LocomproContext> contextOptions = new DbContextOptions<LocomproContext>();
        
        this._logger = new Mock<ILoggerFactory>();
        
        // prepare db sets
        this._submissionDbSet = new Mock<DbSet<Submission>>();
        this._productDbSet = new Mock<DbSet<Product>>();
        this._userDbSet = new Mock<DbSet<User>>();
        this._storeDbSet = new Mock<DbSet<Store>>();
        
        // prepare context and add db sets
        this._context = new Mock<LocomproContext>(contextOptions);
        this._context.Setup(c => c.Set<Submission>()).Returns(this._submissionDbSet.Object);
        this._context.Setup(c => c.Set<Product>()).Returns(this._productDbSet.Object);
        this._context.Setup(c => c.Set<User>()).Returns(this._userDbSet.Object);
        this._context.Setup(c => c.Set<Store>()).Returns(this._storeDbSet.Object);
        
        // set repositories and services
        this._submissionRepository = new SubmissionRepository(this._context.Object, this._logger.Object);
        this._searchService = new SearchService(this._submissionRepository);
        
        this.PrepareDatabase();
    }

    /// <summary>
    /// Finds a list of names that are expected to be found
    /// </summary>
    [Test]
    public void SearchByName_NameIsFound()
    {
        // Arrange
        string productName = "Product1";

        List<Item> searchResults = _searchService.SearchItems(productName, null, null, 0, 0, null, null).Result;
        
        // Assert
        Assert.IsNotNull(searchResults);
    }
    
    /// <summary>
    /// Searches for name that is not expected to be found and
    /// returns empty list
    /// </summary>
    [Test]
    public void SearchByNameForNonExistent_NameIsNotFound()
    {
        
    }
    
    /// <summary>
    /// Gives an empty string and returns empty list
    /// Should not throw an exception
    /// </summary>
    [Test]
    public void SearchByNameForEmptyString_NameIsNotFound()
    {
        
    }
    
    /// <summary>
    /// Searches for an item and the result is the one expected
    /// to be the best submission
    /// </summary>
    [Test]
    public void SearchByName_FindsBestSubmission()
    {
        
    }

    void PrepareDatabase()
    {
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
        
        this._userDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.AsQueryable().Provider);
        this._userDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.AsQueryable().Expression);
        this._userDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.AsQueryable().ElementType);
        this._userDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

        // Add stores
        List<Store> stores = new List<Store>
        {
            new Store
            {
                Name = "Store1",
                Canton = new Canton {Name = "Canton1", ProvinceName = "Province1"}
            },
            new Store
            {
                Name = "Store2",
                Canton = new Canton {Name = "Canton1", ProvinceName = "Province1"}
            },
            new Store
            {
                Name = "Store3",
                Canton = new Canton {Name = "Canton2", ProvinceName = "Province2"}
            },
            new Store
            {
                Name = "Store4",
                Canton = new Canton {Name = "Canton2", ProvinceName = "Province2"}
            }
        };
        
        this._storeDbSet.As<IQueryable<Store>>().Setup(m => m.Provider).Returns(stores.AsQueryable().Provider);
        this._storeDbSet.As<IQueryable<Store>>().Setup(m => m.Expression).Returns(stores.AsQueryable().Expression);
        this._storeDbSet.As<IQueryable<Store>>().Setup(m => m.ElementType).Returns(stores.AsQueryable().ElementType);
        this._storeDbSet.As<IQueryable<Store>>().Setup(m => m.GetEnumerator()).Returns(stores.AsQueryable().GetEnumerator());
        

        // Add products
        List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Product1",
            },
            new Product
            {
                Id = 2,
                Name = "Product2"
            },
            new Product
            {
                Id = 3,
                Name = "Product3"
            },
            new Product
            {
                Id = 4,
                Name = "Product4"
            },
            new Product
            {
                Id = 5,
                Name = "Product5"
            },
            new Product
            {
                Id = 6,
                Name = "Product6"
            },
            new Product
            {
                Id = 7,
                Name = "Product7"
            }
        };
        
        this._productDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.AsQueryable().Provider);
        this._productDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.AsQueryable().Expression);
        this._productDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.AsQueryable().ElementType);
        this._productDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.AsQueryable().GetEnumerator());

        // Add submissions
        List<Submission> submissions = new List<Submission> 
        {
            new Submission
            {
                Username = "User1",
                EntryTime = DateTime.Now.AddDays(-2),
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
                Username = "User2",
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
                Username = "User3",
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
                Username = "User4",
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
                Username = "User5",
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
                Username = "User6",
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
                Username = "User7",
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
                Username = "User8",
                EntryTime = DateTime.Now.AddDays(-8),
                Price = 180,
                Rating = 4.3f,
                Description = "Description for Submission 8",
                StoreName = "Store8",
                ProductId = 8,
                User = users[1],
                Store = stores[1],
                Product = products[0]
            },
            new Submission
            {
                Username = "User9",
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
                Username = "User10",
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
                Username = "User11",
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
                Username = "User12",
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
                Username = "User13",
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
                Username = "User14",
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
                Username = "User15",
                EntryTime = DateTime.Now.AddDays(-15),
                Price = 190,
                Rating = 4.2f,
                Description = "Description for Submission 15",
                StoreName = "Store15",
                ProductId = 15,
                User = users[2],
                Store = stores[2],
                Product = products[0]
            },
            new Submission
            {
                Username = "User16",
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
                Username = "User17",
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
                Username = "User18",
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
                Username = "User19",
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
                Username = "User20",
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
        
        this._submissionDbSet.As<IQueryable<Submission>>().Setup(m => m.Provider).Returns(submissions.AsQueryable().Provider);
        this._submissionDbSet.As<IQueryable<Submission>>().Setup(m => m.Expression).Returns(submissions.AsQueryable().Expression);
        this._submissionDbSet.As<IQueryable<Submission>>().Setup(m => m.ElementType).Returns(submissions.AsQueryable().ElementType);
        this._submissionDbSet.As<IQueryable<Submission>>().Setup(m => m.GetEnumerator()).Returns(submissions.AsQueryable().GetEnumerator());
    }
}