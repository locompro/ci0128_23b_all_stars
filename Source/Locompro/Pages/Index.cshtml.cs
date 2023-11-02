using Locompro.Services;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Locompro.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Locompro.Pages.Shared;

namespace Locompro.Pages
{
    /// <summary>
    /// Index page model
    /// </summary>
    public class IndexModel : SearchPageModel
    {
        public IndexModel(AdvancedSearchInputService advancedSearchServiceHandler,
            IHttpContextAccessor httpContextAccessor)
            : base(advancedSearchServiceHandler, httpContextAccessor)
        {
        }
    }
}