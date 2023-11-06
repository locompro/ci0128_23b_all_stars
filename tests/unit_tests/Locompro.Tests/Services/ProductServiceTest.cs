using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Services.Domain;
using Moq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locompro.Tests.Services
{
    /// <summary>
    /// Provides tests for the ProductService class methods.
    /// </summary>
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductService _productService;

        /// <summary>
        /// Setup method to initialize mocks and product service before each test.
        /// </summary>
        /// <author>Brandon Alonso Mora Umaña</author>
        [SetUp]
        public void Setup()
        {
            // Mock the product repository
            _mockProductRepository = new Mock<IProductRepository>();

            // Mock unit of work (if methods we're testing used it, we would set it up)

            // Mock logger factory (we use a NullLoggerFactory as we're not testing logging here)
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            // Instantiate the service with the mocked dependencies
            _productService = new ProductService(loggerFactory, _mockProductRepository.Object);
        }

        /// <summary>
        /// Tests that GetBrandsAsync returns a list of brands when they exist.
        /// </summary>
        /// <author>Brandon Alonso Mora Umaña</author>
        [Test]
        public async Task GetBrandsAsync_ShouldReturnBrands_WhenBrandsExist()
        {
            // Arrange
            var expectedBrands = new List<string> { "Brand1", "Brand2", "Brand3" };
            _mockProductRepository.Setup(repo => repo.GetBrandsAsync()).ReturnsAsync(expectedBrands);

            // Act
            var brands = await _productService.GetBrandsAsync();

            // Assert
            Assert.That(brands, Is.EquivalentTo(expectedBrands));
            _mockProductRepository.Verify(repo => repo.GetBrandsAsync(), Times.Once);
        }

        /// <summary>
        /// Tests that GetModelsAsync returns a list of models when they exist.
        /// </summary>
        /// <author>Brandon Alonso Mora Umaña</author>
        [Test]
        public async Task GetModelsAsync_ShouldReturnModels_WhenModelsExist()
        {
            // Arrange
            var expectedModels = new List<string> { "Model1", "Model2", "Model3" };
            _mockProductRepository.Setup(repo => repo.GetModelsAsync()).ReturnsAsync(expectedModels);

            // Act
            var models = await _productService.GetModelsAsync();

            // Assert
            Assert.That(models, Is.EquivalentTo(expectedModels));
            _mockProductRepository.Verify(repo => repo.GetModelsAsync(), Times.Once);
        }
    }
}