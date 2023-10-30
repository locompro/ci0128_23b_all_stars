using Locompro.Data;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class ContributionServiceTests
{
    private ContributionService _contributionService;

    // Mocks for your dependencies
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ICantonService> _cantonService;
    private Mock<INamedEntityDomainService<Store, string>> _storeService;
    private Mock<INamedEntityDomainService<Product, int>> _productService;
    private Mock<INamedEntityDomainService<Category, string>> _categoryService;
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

        Mock<ILoggerFactory> loggerFactory = new Mock<ILoggerFactory>();

        _contributionService = new ContributionService(_unitOfWork.Object, loggerFactory.Object, _cantonService.Object,
            _storeService.Object,
            _productService.Object, _categoryService.Object, _submissionService.Object);
    }

    [Test]
    public async Task AddSubmission_CreatesNewSubmission()
    {
        // Arrange
        var storeViewModel = new StoreViewModel();
        var productViewModel = new ProductViewModel();
        string description = "Test Description";
        int price = 100;
        string userId = "TestUser";

        // Setup mock behavior
        _storeService.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(new Store());
        _productService.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(new Product());

        // Act
        await _contributionService.AddSubmission(storeViewModel, productViewModel, description, price, userId);

        // Assert
        _submissionService.Verify(s => s.Add(It.Is<Submission>(sub =>
            sub.UserId == userId &&
            sub.Price == price &&
            sub.Description == description &&
            sub.Store != null &&
            sub.Product != null
        )), Times.Once);
    }
}