using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Pages.Util;

/// <summary>
/// Handles the parsing and serialization of pictures
/// </summary>
public static class PictureParser
{
    /// <summary>
    /// Parses a list of files in form format to a list of pictures
    /// </summary>
    /// <param name="uploadedFiles"></param>
    /// <returns></returns>
    public static List<PictureVm> Parse(IFormFileCollection uploadedFiles)
    {
        List<PictureVm> pictures = new List<PictureVm>();
        
        foreach(IFormFile file in uploadedFiles)
        {
            if (IsValidImage(file))
            {
                PictureVm picture = ParseSinglePicture(file);
            
                pictures.Add(picture);
            }
        }
        
        return pictures;
    }
    
    /// <summary>
    /// Checks if the file has a valid extension
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static bool IsValidImage(IFormFile file)
    {
        string fileName = file.FileName;
        
        string extension = Path.GetExtension(fileName);
        
        return extension is ".jpg" or ".jpeg" or ".png";
    }
    
    /// <summary>
    /// Parses a single file in form format to a picture
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static PictureVm ParseSinglePicture(IFormFile file)
    {
        MemoryStream ms = new MemoryStream();
            
        file.CopyTo(ms);
        PictureVm picture = new PictureVm
        {
            Name = file.FileName,
            PictureData = ms.ToArray()
        };
        
        ms.Close();
        ms.Dispose();

        return picture;
    }

    /// <summary>
    /// Serializes a list of pictures to a list of strings
    /// </summary>
    /// <param name="pictures"></param>
    /// <returns></returns>
    public static List<string> Serialize(List<Picture> pictures)
    {
        return pictures.Select(SerializeSinglePicture).ToList();
    }
    
    /// <summary>
    /// Serializes a single picture to a string
    /// </summary>
    /// <param name="picture"></param>
    /// <returns></returns>
    private static string SerializeSinglePicture(Picture picture)
    {
        byte[] unserializedData = picture.PictureData;

        return SerializeData(unserializedData);
    }
    
    /// <summary>
    /// Converts a byte array to a string that can be displayed by browsers
    /// </summary>
    /// <param name="unserializedData"></param>
    /// <returns></returns>
    public static string SerializeData(byte[] unserializedData)
    {
        return $"data:image/png;base64,{Convert.ToBase64String(unserializedData)}";
    } 
    
}