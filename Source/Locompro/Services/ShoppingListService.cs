using Locompro.Models.Dtos;
using Locompro.Services.Auth;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class ShoppingListService : Service, IShoppingListService
{
    private IUserService _userService;

    private ISubmissionService _submissionService;

    private IAuthService _authService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="userService"></param>
    /// <param name="submissionService"></param>
    /// <param name="authService"></param>
    public ShoppingListService(ILoggerFactory loggerFactory, IUserService userService,
        ISubmissionService submissionService, IAuthService authService) : base(loggerFactory)
    {
        _userService = userService;
        _submissionService = submissionService;
        _authService = authService;
    }

    /// <inheritdoc />
    public Task<ShoppingListDto> Get()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ShoppingListSummaryDto> GetStoreSummary()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task AddProduct()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task DeleteProduct()
    {
        throw new NotImplementedException();
    }
}