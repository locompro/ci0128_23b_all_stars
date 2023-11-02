using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Pages.Util;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class PicturesServiceTest
{
    private PicturesService _picturesService;
    
    // Mocks for your dependencies
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ICantonService> _cantonService;
    private Mock<INamedEntityDomainService<Store, string>> _storeService;
    private Mock<INamedEntityDomainService<Product, int>> _productService;
    private Mock<INamedEntityDomainService<Category, string>> _categoryService;
    private Mock<IPicturesRepository> _picturesRepository;
    private Mock<ISubmissionService> _submissionService;
    
    [SetUp]
    public void Setup()
    {
        // Initialize your mocks here
        _unitOfWork = new Mock<IUnitOfWork>();
        _cantonService = new Mock<ICantonService>();
        _storeService = new Mock<INamedEntityDomainService<Store, string>>();
        _productService = new Mock<INamedEntityDomainService<Product, int>>();
        _categoryService = new Mock<INamedEntityDomainService<Category, string>>();
        _submissionService = new Mock<ISubmissionService>();
        _picturesRepository = new Mock<IPicturesRepository>();
        
        _unitOfWork.Setup(u => u.GetRepository<IPicturesRepository>()).Returns(_picturesRepository.Object);

        Mock<ILoggerFactory> loggerFactory = new Mock<ILoggerFactory>();

        _picturesService = new PicturesService(_unitOfWork.Object, loggerFactory.Object);
    }
    
    [Test]
    public async Task GetPicturesForItem_ReturnsPictures()
    {
        // Arrange
        Category category = new Category()
        {
            Name = "Test Category"
        };
        
        Store store = new Store()
        {
            Name = "Test Store",
            Canton = new Canton()
            {
                Name = "Test Canton"
            }
        };
        
        Product product = new Product()
        {
            Id = 1,
            Name = "Test Product",
            Categories = new List<Category>()
            {
              category  
            },
        };
        
        Submission submission = new Submission()
        {
            Product = product,
            Store = store,
            UserId = "Test User",
            EntryTime = DateTime.Now,
            Price = 100,
            Description = "Test Description"
        };
        
        product.Submissions = new List<Submission>()
        {
            submission
        };

        Picture picture = new Picture()
        {
            SubmissionUserId = submission.UserId,
            SubmissionEntryTime = submission.EntryTime,
            Index = 1,
            PictureTitle = "pictureTitle",
            Submission = submission
        };
        
        var pictureAmount = 1;
        var productName = "Test Product";
        var storeName = "Test Store";
        
        List<Submission> submissions = new List<Submission>()
        {
            submission
        };
        
        List<Picture> pictures = new List<Picture>()
        {
            picture
        };
        
        // Setup mock behavior
        _picturesRepository.Setup(p => p.GetPicturesByItem(It.IsAny<int>() ,It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((int pictureAmount, string productName, string storeName) =>
            {
                return pictures.Where(p => p.Submission.Product.Name == productName)
                    .Where(p => p.Submission.Store.Name == storeName)
                    .Take(pictureAmount).ToList();
            }
        );
        
        // Act
        var result = await _picturesService.GetPicturesForItem(pictureAmount, productName, storeName);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.AreEqual(pictures.Count, result.Count);
            Assert.AreEqual(pictures[0].SubmissionUserId, result[0].SubmissionUserId);
            Assert.AreEqual(pictures[0].SubmissionEntryTime, result[0].SubmissionEntryTime);
            Assert.AreEqual(pictures[0].Index, result[0].Index);
            Assert.AreEqual(pictures[0].PictureTitle, result[0].PictureTitle);
            Assert.AreEqual(pictures[0].Submission, result[0].Submission);
        });
    }
}