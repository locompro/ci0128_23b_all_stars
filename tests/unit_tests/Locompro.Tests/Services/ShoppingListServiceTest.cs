using Locompro.Common.Mappers;
using Locompro.Migrations;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Factories;
using Locompro.Services;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class ShoppingListServiceTest
{
    private Mock<IAuthService> _authService;
    
    private Mock<IUserService> _userService;
    
    private ShoppingListService _shoppingListService;

    private List<User> _users;
    
    [SetUp]
    public void SetUp()
    {
        ILoggerFactory loggerFactory = new LoggerFactory();
        
        _authService = new Mock<IAuthService>();
        _userService = new Mock<IUserService>();
        
        _shoppingListService = new ShoppingListService(
            loggerFactory,
            _authService.Object,
            _userService.Object);

        _userService
            .Setup(userService => userService.GetShoppingList(It.IsAny<string>()))
            .ReturnsAsync((string userId) =>
            {
                var user = _users.Find(u => u.Id == userId);
                if (user == null) throw new Exception("User not found");
                
                List<Product> shoppingList = user.ShoppedProducts.ToList();
        
                ShoppingListProductFactory factory = new ShoppingListProductFactory();
        
                return new ShoppingListDto
                {
                    UserId = userId,
                    Products = shoppingList.Select(product => factory.ToDto(product)).ToList()
                };
            });

        _userService
            .Setup(userService => userService.AddProductToShoppingList(It.IsAny<string>(), It.IsAny<int>()))
            .Returns((string userId, int productId) =>
            {
                var user = _users.Find(u => u.Id == userId);
                if (user == null) throw new Exception("User not found");
                
                Product newProduct = new Product
                {
                    Id = productId
                };
                
                user.ShoppedProducts.Add(newProduct);
                
                return Task.CompletedTask;
            });

        _userService
            .Setup(userService => userService.DeleteProductFromShoppingList(It.IsAny<string>(), It.IsAny<int>()))
            .Returns((string userId, int productId) =>
            {
                var user = _users.Find(u => u.Id == userId);
                if (user == null) throw new Exception("User not found");
                
                Product? product = user.ShoppedProducts.FirstOrDefault(p => p.Id == productId);
                
                user.ShoppedProducts.Remove(product);
                
                return Task.CompletedTask;
            });
        
        _authService
            .Setup(authService => authService.GetUserId())
            .Returns(() => _users[0].Id);

        _users = new List<User>
        {
            new User()
            {
                UserName = "UserName1",
                ShoppedProducts = new List<Product>()
            },
            new User()
            {
                UserName = "UserName2",
                ShoppedProducts = new List<Product>()
            }
        };
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Test]
    public async Task GetReturnsShoppingList()
    {
        ClearUsersShoppingLists();
        
        _users[0].ShoppedProducts.Add(new Product
        {
            Id = 1
        });
        _users[0].ShoppedProducts.Add(new Product
        {
            Id = 2
        });

        var shoppingListDto = await _shoppingListService.Get();
        
        var mapper = new ShoppingListMapper();
        var shoppingListVm = mapper.ToVm(shoppingListDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(shoppingListVm.UserId, Is.EqualTo(_users[0].Id));
            Assert.That(shoppingListVm.Products, Is.Not.Null);
            Assert.That(shoppingListVm.Products.Count, Is.EqualTo(2));
        });
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void GetOnNullUserThrowsException()
    {
        ClearUsersShoppingLists();
        
        _authService
            .Setup(authService => authService.GetUserId())
            .Returns(() => null);
        
        Assert.ThrowsAsync<Exception>(async () => await _shoppingListService.Get());
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Test]
    public async Task GetReturnsEmptyShoppingList()
    {
        ClearUsersShoppingLists();
        
        var shoppingListDto = await _shoppingListService.Get();
        
        var mapper = new ShoppingListMapper();
        var shoppingListVm = mapper.ToVm(shoppingListDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(shoppingListVm.UserId, Is.EqualTo(_users[0].Id));
            Assert.That(shoppingListVm.Products, Is.Not.Null);
            Assert.That(shoppingListVm.Products.Count, Is.EqualTo(0));
        });
    }

    /// <summary>
    /// 
    /// </summary>
    [Test]
    public async Task AddProductAddsProductToShoppingList()
    {
        ClearUsersShoppingLists();
        
        await _shoppingListService.AddProduct(1);
        
        Assert.That(_users[0].ShoppedProducts, Has.Count.EqualTo(1));
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void AddProductOnNullUserThrowsException()
    {
        ClearUsersShoppingLists();
        
        _authService
            .Setup(authService => authService.GetUserId())
            .Returns(() => null);
        
        Assert.ThrowsAsync<Exception>(async () => await _shoppingListService.AddProduct(1));
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Test]
    public async Task DeleteProductDeletesProductFromShoppingList()
    {
        ClearUsersShoppingLists();
        
        _users[0].ShoppedProducts.Add(new Product
        {
            Id = 1
        });
        _users[0].ShoppedProducts.Add(new Product
        {
            Id = 2
        });
        
        await _shoppingListService.DeleteProduct(1);
        
        Assert.That(_users[0].ShoppedProducts, Has.Count.EqualTo(1));
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void DeleteProductOnNullUserThrowsException()
    {
        ClearUsersShoppingLists();
        
        _authService
            .Setup(authService => authService.GetUserId())
            .Returns(() => null);
        
        Assert.ThrowsAsync<Exception>(async () => await _shoppingListService.DeleteProduct(1));
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void ClearUsersShoppingLists()
    {
        foreach (var user in _users)
        {
            user.ShoppedProducts.Clear();
        }
    }
}