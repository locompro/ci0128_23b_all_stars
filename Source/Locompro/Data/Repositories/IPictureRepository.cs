using Locompro.Models.Entities;
using Locompro.Models.Results;
using Locompro.Services.Domain;

namespace Locompro.Data.Repositories;


public interface IPictureRepository : ICrudRepository<Picture, PictureKey>
{
    Task<List<GetPicturesResult>> GetPicturesByItem(int pictureAmount, int productId, string storeName);
}