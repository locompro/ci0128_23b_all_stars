using Locompro.Models;
using Locompro.Models.ViewModels;

namespace Locompro.Pages.Util;

public static class PictureParser
{
    public static List<PictureViewModel> Parse(IFormFileCollection uploadedFiles)
    {
        List<PictureViewModel> pictures = new List<PictureViewModel>();
        
        foreach(IFormFile file in uploadedFiles)
        {
            if (IsValidImage(file))
            {
                PictureViewModel picture = GetPictureFromFile(file);
            
                pictures.Add(picture);
            }
        }
        
        return pictures;
    }
    
    private static bool IsValidImage(IFormFile file)
    {
        string fileName = file.FileName;
        
        string extension = Path.GetExtension(fileName);
        
        return extension is ".jpg" or ".jpeg" or ".png";
    }

    private static PictureViewModel GetPictureFromFile(IFormFile file)
    {
        MemoryStream ms = new MemoryStream();
            
        file.CopyTo(ms);
        PictureViewModel picture = new PictureViewModel
        {
            Name = file.FileName,
            PictureData = ms.ToArray()
        };
        
        ms.Close();
        ms.Dispose();

        return picture;
    }
}