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
        this._logger = new Mock<ILoggerFactory>();
        
        // prepare db sets
        this._submissionDbSet = new Mock<DbSet<Submission>>();
        this._productDbSet = new Mock<DbSet<Product>>();
        this._userDbSet = new Mock<DbSet<User>>();
        this._storeDbSet = new Mock<DbSet<Store>>();
        
        // prepare context and add db sets
        this._context = new Mock<LocomproContext>();
        this._context.Setup(c => c.Set<Submission>()).Returns(this._submissionDbSet.Object);
        this._context.Setup(c => c.Set<Product>()).Returns(this._productDbSet.Object);
        this._context.Setup(c => c.Set<User>()).Returns(this._userDbSet.Object);
        this._context.Setup(c => c.Set<Store>()).Returns(this._storeDbSet.Object);
        
        // set repositories and services
        this._submissionRepository = new SubmissionRepository(this._context.Object, this._logger.Object);
        this._searchService = new SearchService(this._submissionRepository);
        
        // Add users

        // Add stores

        // Add products

        // Add submissions 


    }

    /// <summary>
    /// Finds a list of names that are expected to be found
    /// </summary>
    [Test]
    public void SearchByName_NameIsFound()
    {
        
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
    
}