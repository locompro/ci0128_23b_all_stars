using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class PictureServiceTest
{
    [SetUp]
    public void Setup()
    {
        // Initialize your mocks here
        _unitOfWork = new Mock<IUnitOfWork>();
        _picturesRepository = new Mock<IPictureRepository>();

        _unitOfWork.Setup(u => u.GetSpecialRepository<IPictureRepository>()).Returns(_picturesRepository.Object);

        var loggerFactory = new Mock<ILoggerFactory>();

        _pictureService = new PictureService(_unitOfWork.Object, loggerFactory.Object);
    }

    private PictureService? _pictureService;

    // Mocks for your dependencies
    private Mock<IUnitOfWork>? _unitOfWork;
    private Mock<IPictureRepository>? _picturesRepository;

    /// <summary>
    ///    Test to ensure GetPicturesForItem returns expected pictures.
    /// </summary>
    /// <author> Joseph Valverde Kong - Sprint 2 </author>
    [Test]
    public async Task GetPicturesForItem_ReturnsPictures()
    {
        // Arrange
        var category = new Category
        {
            Name = "Test Category"
        };

        var store = new Store
        {
            Name = "Test Store",
            Canton = new Canton
            {
                Name = "Test Canton"
            }
        };

        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Categories = new List<Category>
            {
                category
            }
        };

        var submission = new Submission
        {
            Product = product,
            Store = store,
            UserId = "Test User",
            EntryTime = DateTime.Now,
            Price = 100,
            Description = "Test Description"
        };

        product.Submissions = new List<Submission>
        {
            submission
        };

        var picture = new Picture
        {
            SubmissionUserId = submission.UserId,
            SubmissionEntryTime = submission.EntryTime,
            Index = 1,
            PictureTitle = "pictureTitle",
            Submission = submission
        };

        var testPictureAmount = 1;
        var testProductName = "Test Product";
        var testStoreName = "Test Store";

        var submissions = new List<Submission>
        {
            submission
        };

        var pictures = new List<Picture>
        {
            picture
        };

        // Setup mock behavior
        _picturesRepository!.Setup(p => p.GetPicturesByItem(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((int pictureAmount, string productName, string storeName) =>
                {
                    return pictures.Where(p => p.Submission.Product.Name == productName)
                        .Where(p => p.Submission.Store.Name == storeName)
                        .Take(pictureAmount).ToList();
                }
            );

        // Act
        var result = await _pictureService!.GetPicturesForItem(testPictureAmount, testProductName, testStoreName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(pictures.Count));
            Assert.That(result[0].SubmissionUserId, Is.EqualTo(pictures[0].SubmissionUserId));
            Assert.That(result[0].SubmissionEntryTime, Is.EqualTo(pictures[0].SubmissionEntryTime));
            Assert.That(result[0].Index, Is.EqualTo(pictures[0].Index));
            Assert.That(result[0].PictureTitle, Is.EqualTo(pictures[0].PictureTitle));
            Assert.That(result[0].Submission, Is.EqualTo(pictures[0].Submission));
        });
    }
}