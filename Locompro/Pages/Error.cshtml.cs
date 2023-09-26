using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace locompro.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public string text { get; set; }

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // from here on out example on how use the parameters for normal and advanced search
            string searchString = Request.Query["normalSearch"];

            if (searchString == null)
            {
                this.text = "advanced search: ";
                searchString = Request.Query["advancedSearch"];
            } else
            {
                this.text = "normal search: ";
            }

            this.text += searchString;

            Console.WriteLine(text);
        }
    }
}