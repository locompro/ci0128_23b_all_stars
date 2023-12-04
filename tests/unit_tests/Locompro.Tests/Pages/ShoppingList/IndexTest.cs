using Locompro.Models.Dtos;
using Locompro.Pages.ShoppingList;
using Locompro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Pages.ShoppingList
{
    /// <summary>
    ///     This class contains unit tests for the ShoppingListModel class.
    /// </summary>
    [TestFixture]
    public class ShoppingListModelTest
    {
        private ILoggerFactory _loggerFactory;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IShoppingListService> _shoppingListServiceMock;
        private ShoppingListModel _shoppingListModel;

        [SetUp]
        public void SetUp()
        {
            _loggerFactory = new LoggerFactory();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _shoppingListServiceMock = new Mock<IShoppingListService>();

            _shoppingListModel = new ShoppingListModel(_loggerFactory, _httpContextAccessorMock.Object, _shoppingListServiceMock.Object);
        }

        /// <summary>
        /// Test if OnGetAsync successfully loads shopping list data.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnGetAsync_LoadsShoppingListData()
        {
            // Arrange
            var shoppingListDto = new ShoppingListDto{ UserId = "a", Products = new List<ShoppingListProductDto>()};
            var shoppingListSummaryDto = new ShoppingListSummaryDto{ UserId = "a", Stores = new List<ShoppingListSummaryStoreDto>()};
            _shoppingListServiceMock.Setup(s => s.Get()).ReturnsAsync(shoppingListDto);
            _shoppingListServiceMock.Setup(s => s.GetSummary()).ReturnsAsync(shoppingListSummaryDto);

            // Act
            await _shoppingListModel.OnGetAsync();

            // Assert
            Assert.IsNotNull(_shoppingListModel.ShoppingListVm);
        }

        /// <summary>
        /// Test if OnGetAsync successfully loads shopping list summary data.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnGetAsync_LoadsShoppingListSummaryData()
        {
            // Arrange
            var shoppingListDto = new ShoppingListDto{ UserId = "a", Products = new List<ShoppingListProductDto>()};
            var shoppingListSummaryDto = new ShoppingListSummaryDto{ UserId = "a", Stores = new List<ShoppingListSummaryStoreDto>()};
            _shoppingListServiceMock.Setup(s => s.Get()).ReturnsAsync(shoppingListDto);
            _shoppingListServiceMock.Setup(s => s.GetSummary()).ReturnsAsync(shoppingListSummaryDto);

            // Act
            await _shoppingListModel.OnGetAsync();

            // Assert
            Assert.IsNotNull(_shoppingListModel.ShoppingListSummaryVm);
        }

        /// <summary>
        /// Test if OnPostAddProduct successfully adds a product.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnPostAddProduct_AddsProductSuccessfully()
        {
            // Arrange
            int productId = 123; // Example product ID

            // Act
            await _shoppingListModel.OnPostAddProduct(productId);

            // Assert
            _shoppingListServiceMock.Verify(s => s.AddProduct(productId), Times.Once);
        }

        /// <summary>
        /// Test if OnPostDeleteProduct successfully deletes a product.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnPostDeleteProduct_DeletesProductSuccessfully()
        {
            // Arrange
            int productId = 123; // Example product ID

            // Act
            await _shoppingListModel.OnPostDeleteProduct(productId);

            // Assert
            _shoppingListServiceMock.Verify(s => s.DeleteProduct(productId), Times.Once);
        }
        
        /// <summary>
        /// Test if OnGetAsync handles exceptions correctly.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnGetAsync_ExceptionThrown_HandlesException()
        {
            // Arrange
            _shoppingListServiceMock.Setup(s => s.Get()).ThrowsAsync(new Exception("Test Exception"));

            // Act
            await _shoppingListModel.OnGetAsync();

            // Assert
            Assert.IsNotNull(_shoppingListModel.ShoppingListVm);
            Assert.IsNotNull(_shoppingListModel.ShoppingListSummaryVm);
        }

        /// <summary>
        /// Test if OnPostAddProduct handles exceptions correctly.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnPostAddProduct_ExceptionThrown_ReturnsErrorJson()
        {
            // Arrange
            int productId = 123; // Example product ID
            _shoppingListServiceMock.Setup(s => s.AddProduct(productId)).ThrowsAsync(new Exception("Test Exception"));

            // Act
            JsonResult result = await _shoppingListModel.OnPostAddProduct(productId);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(500));
        }

        /// <summary>
        /// Test if OnPostDeleteProduct handles exceptions correctly.
        /// </summary>
        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public async Task OnPostDeleteProduct_ExceptionThrown_ReturnsErrorJson()
        {
            // Arrange
            int productId = 123; // Example product ID
            _shoppingListServiceMock.Setup(s => s.DeleteProduct(productId)).ThrowsAsync(new Exception("Test Exception"));

            // Act
            JsonResult result = await _shoppingListModel.OnPostDeleteProduct(productId);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(500));
        }
    }
}