using Locompro.Data;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class ContributionServiceTests
{
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

        var loggerFactory = new Mock<ILoggerFactory>();

        _contributionService = new ContributionService(_unitOfWork.Object, loggerFactory.Object, _cantonService.Object,
            _storeService.Object,
            _productService.Object, _categoryService.Object, _submissionService.Object);
    }

    private ContributionService _contributionService;

    // Mocks for your dependencies
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ICantonService> _cantonService;
    private Mock<INamedEntityDomainService<Store, string>> _storeService;
    private Mock<INamedEntityDomainService<Product, int>> _productService;
    private Mock<INamedEntityDomainService<Category, string>> _categoryService;
    private Mock<ISubmissionService> _submissionService;

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task AddSubmission_CreatesNewSubmission()
    {
        // Arrange
        var storeViewModel = new StoreVm();
        var productViewModel = new ProductVm();

        // Setup mock behavior
        _storeService.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(new Store());
        _productService.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(new Product());

        var submissionVM = new SubmissionVm
        {
            Description = "Test Description",
            Price = 100,
            UserId = "TestUser"
        };

        var pictureVM = new List<PictureVm>();

        // Act
        await _contributionService.AddSubmission(storeViewModel, productViewModel, submissionVM, pictureVM);

        // Assert
        _submissionService.Verify(s => s.Add(It.Is<Submission>(sub =>
            sub.UserId == submissionVM.UserId &&
            sub.Price == submissionVM.Price &&
            sub.Description == submissionVM.Description &&
            sub.Store != null &&
            sub.Product != null &&
            sub.Pictures != null
        )), Times.Once);
    }

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task AddSubmission_AddsPictures()
    {
        // Arrange
        var storeViewModel = new StoreVm();
        var productViewModel = new ProductVm();

        // Setup mock behavior
        _storeService.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(new Store());
        _productService.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(new Product());

        var submissionVM = new SubmissionVm
        {
            Description = "Test Description",
            Price = 100,
            UserId = "TestUser"
        };

        var pictureVM = new List<PictureVm>
        {
            new()
            {
                Name = "Pic1",
                PictureData = new byte[] { 0, 1, 2, 3, 4 }
            },
            new()
            {
                Name = "Pic2",
                PictureData = new byte[] { 0, 1, 2, 3, 4 }
            },
            new()
            {
                Name = "Pic3",
                PictureData = new byte[] { 0, 1, 2, 3, 4 }
            }
        };

        // Act
        await _contributionService.AddSubmission(storeViewModel, productViewModel, submissionVM, pictureVM);

        // Assert
        _submissionService.Verify(s => s.Add(It.Is<Submission>(sub =>
            sub.Pictures.Count == pictureVM.Count &&
            sub.Pictures.ToList()[0].PictureTitle == pictureVM[0].Name &&
            sub.Pictures.ToList()[1].PictureTitle == pictureVM[1].Name &&
            sub.Pictures.ToList()[2].PictureTitle == pictureVM[2].Name &&
            sub.Pictures.ToList()[0].PictureData == pictureVM[0].PictureData &&
            sub.Pictures.ToList()[1].PictureData == pictureVM[1].PictureData &&
            sub.Pictures.ToList()[2].PictureData == pictureVM[2].PictureData
        )), Times.Once);
    }
}