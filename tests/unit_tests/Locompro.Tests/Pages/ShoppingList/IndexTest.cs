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
    }
}