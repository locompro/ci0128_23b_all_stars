using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Locompro.Pages
{
    /// <summary>
    /// Index page model
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// string for search query product name
        /// </summary>
        public string SearchQuery { get; set; }

    }
}