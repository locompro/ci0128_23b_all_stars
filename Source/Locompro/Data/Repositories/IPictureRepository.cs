using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Services;
using Locompro.Services.Domain;

namespace Locompro.Data.Repositories;

public interface IPictureRepository : ICrudRepository<Picture, PictureKey>
{
    Task<List<Picture>> GetPicturesByItem(int pictureAmount, string productName, string storeName);
}