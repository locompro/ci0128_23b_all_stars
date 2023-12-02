using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

/// <summary>
///     Iterface for services for handling the access of pictures
/// </summary>
public interface IPictureService : IDomainService<Picture, PictureKey>
{
    /// <summary>
    ///     Returns the given pictures for an item described by the product name and the store name
    /// </summary>
    /// <param name="pictureAmount"> Max amount of pictures that it is wished to be retrieved</param>
    /// <param name="productName"> Name of the product that has the pictures</param>
    /// <param name="storeName"> Name of the store where the product is located</param>
    /// <returns> A list of pictures </returns>
    public Task<List<PictureDto>> GetPicturesForItem(int pictureAmount, int productId, string storeName);
}