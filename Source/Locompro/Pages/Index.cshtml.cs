using Locompro.Common;
using Locompro.Pages.Shared;
using Locompro.Services;

namespace Locompro.Pages;

/// <summary>
///     Index page model
/// </summary>
public class IndexModel : SearchPageModel
{
    public IndexModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        AdvancedSearchInputService advancedSearchServiceHandler,
        IApiKeyHandler apiKeyHandler)
        : base(loggerFactory, httpContextAccessor, advancedSearchServiceHandler, apiKeyHandler)
    {
    }
}