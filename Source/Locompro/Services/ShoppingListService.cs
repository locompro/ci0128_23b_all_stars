using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Factories;
using Locompro.Services.Auth;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class ShoppingListService : Service, IShoppingListService
{
    private readonly IAuthService _authService;
    
    private readonly IUserService _userService;
    
    private readonly IDomainService<Product, int> _productService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="submissionService"></param>
    /// <param name="authService"></param>
    /// <param name="managerService"></param>
    /// <param name="userService"></param>
    /// <param name="productService"></param>
    public ShoppingListService(ILoggerFactory loggerFactory,
        IAuthService authService,
        IUserService userService,
        IDomainService<Product, int> productService)
        : base(loggerFactory)
    {
        _authService = authService;
        _userService = userService;
        _productService = productService;
    }

    /// <inheritdoc />
    public async Task<ShoppingListDto> Get()
    {
        var userId = _authService.GetUserId();
        if (userId == null) throw new Exception("User not found");

        return await _userService.GetShoppingList(userId);
    }

    /// <inheritdoc />
    public Task<ShoppingListSummaryDto> GetStoreSummary()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task AddProduct(int productId)
    {
        var userId = _authService.GetUserId();
        if (userId == null) throw new Exception("User not found");
        
        await _userService.AddProductToShoppingList(userId, productId);
    }

    /// <inheritdoc />
    public async Task DeleteProduct(int productId)
    {
        var userId = _authService.GetUserId();
        if (userId == null) throw new Exception("User not found");
        
        await _userService.DeleteProductFromShoppingList(userId, productId);
    }
}