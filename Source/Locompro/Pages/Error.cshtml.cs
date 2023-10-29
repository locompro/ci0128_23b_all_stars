using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using Locompro.Common;
using Microsoft.Extensions.Logging;

namespace Locompro.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    public List<Image> Images { get; set; }

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
        Images = new List<Image>();
    }

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        
        Console.WriteLine("Get request...");
    }
    
    public void OnPost()
    {
        Console.WriteLine("Image post processing...");
        foreach(var file in Request.Form.Files)
        {
            Image image = new Image
            {
                ImageTitle = file.FileName
            };

            MemoryStream ms = new MemoryStream();
            
            file.CopyTo(ms);
            image.ImageData = ms.ToArray();

            ms.Close();
            ms.Dispose();
            
            Images.Add(image);
        }
    }

    public static string GetImageUrl(Image image)
    {
        string imageBase64Data = 
            Convert.ToBase64String(image.ImageData);
            
        string imageDataUrl = 
            string.Format("data:image/png;base64,{0}", 
                imageBase64Data);

        return imageDataUrl;
    }
}