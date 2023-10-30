using System.Drawing;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;
using Locompro.Models;

public interface IPicturesService : IDomainService<Picture, PictureKey>
{
    public List<Picture> GetItemPictures(int pictureAmount, string submissionUserId, DateTime submissionEntryTime);
}