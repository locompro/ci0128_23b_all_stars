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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="submissionService"></param>
    /// <param name="authService"></param>
    /// <param name="managerService"></param>
    public ShoppingListService(ILoggerFactory loggerFactory,
        ISubmissionService submissionService,
        IAuthService authService,
        IUserManagerService managerService,
        IUserService userService)
        : base(loggerFactory)
    {
        _submissionService = submissionService;
        _authService = authService;
        _userManagerService = managerService;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<ShoppingListDto> Get()
    {
        var userId = _authService.GetUserId();
        User user = await _userManagerService.FindByIdAsync(userId);
        
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
        User user = await _userManagerService.FindByIdAsync(userId);

        
    }

    /// <inheritdoc />
    public async Task DeleteProduct(int productId)
    {
        var userId = _authService.GetUserId();
        User user = await _userManagerService.FindByIdAsync(userId);
        
    }
}