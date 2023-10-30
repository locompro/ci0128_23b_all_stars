using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class PictureKey
{
    public string SubmissionUserId { get; set; }
    public DateTime SubmissionEntryTime { get; set; }
    public int Id { get; set; }
}

public class PicturesService : DomainService<Picture, PictureKey>, IPicturesService
{
    public PicturesService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory)
    {
    }

    public List<Picture> GetItemPictures(int pictureAmount, string submissionUserId, DateTime submissionEntryTime)
    {
        return new List<Picture>();
    }
    
    public static string GetImageUrl(Picture picture)
    {
        string imageBase64Data = 
            Convert.ToBase64String(picture.PictureData);
            
        string imageDataUrl = 
            string.Format("data:image/png;base64,{0}", 
                imageBase64Data);

        return imageDataUrl;
    }
}