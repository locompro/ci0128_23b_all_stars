using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace locompro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string SearchQuery { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string SearchString)
        {

        }

        public void OnPost(string SearchString)
        {
            // Get the button that was clicked
            string ButtonClicked = Request.Form["SearchButton"];

            const string searchLink = "Error"; // TODO: change this to the search page

            // If a button was clicked
            if (ButtonClicked != null)
            {
                // Redirect to the appropriate page
                switch (ButtonClicked)
                {
                    // for normal searches
                    case "normalSearchButton":
                        Response.Redirect(searchLink + "?normalSearch=" + SearchString);
                        break;
                    // for advanced searches
                    case "advancedSearchButton":
                        Response.Redirect(searchLink + "?advancedSearch=" + SearchString);
                        break;
                    // for any other kind of search type given which has not been defined
                    default:
                        // do nothing and return
                        Response.Redirect("./Index");
                        break;
                }
            }
            else
            {
                // if button was not clicked, then redirect back
                Response.Redirect("./Index");
            }
        }
    }
}