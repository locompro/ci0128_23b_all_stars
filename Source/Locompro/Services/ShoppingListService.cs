using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Factories;
using Locompro.Services.Auth;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class ShoppingListService : Service, IShoppingListService
{
    private readonly ISubmissionService _submissionService;

    private readonly IAuthService _authService;
    
    private readonly IUserManagerService _userManagerService;
    
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
        ISubmissionService submissionService,
        IAuthService authService,
        IUserManagerService managerService,
        IUserService userService,
        IDomainService<Product, int> productService)
        : base(loggerFactory)
    {
        _submissionService = submissionService;
        _authService = authService;
        _userManagerService = managerService;
        _userService = userService;
        _productService = productService;
    }

    /// <inheritdoc />
    public async Task<ShoppingListDto> Get()
    {
        var userId = _authService.GetUserId();
        if (userId == null) throw new Exception("User not found");
        
        User user = await _userManagerService.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        
        List<Product> shoppingList = user.ShoppedProducts.ToList();
        
        ShoppingListProductFactory factory = new ShoppingListProductFactory();
        
        return new ShoppingListDto
        {
            UserId = userId,
            Products = shoppingList.Select(product => factory.ToDto(product)).ToList()
        };
    }

    /// <inheritdoc />
    public Task<StoreSummaryDto> GetStoreSummary()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task AddProduct(int productId)
    {
        var userId = _authService.GetUserId();
        if (userId == null) throw new Exception("User not found");
        
        User user = await _userManagerService.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        
        Product product = await _productService.Get(productId);
        if (product == null) throw new Exception("Product not found");
        
        user.ShoppedProducts.Add(product);
        await _userService.Update(userId, user);
    }

    /// <inheritdoc />
    public async Task DeleteProduct(int productId)
    {
        var userId = _authService.GetUserId();
        if (userId == null) throw new Exception("User not found");
        
        User user = await _userManagerService.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        
        Product product = await _productService.Get(productId);
        if (product == null) throw new Exception("Product not found");
        
        user.ShoppedProducts.Remove(product);
        await _userService.Update(userId, user);
    }
}