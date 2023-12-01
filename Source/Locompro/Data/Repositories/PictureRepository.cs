using Locompro.Models.Entities;
using Locompro.Models.Results;
using Locompro.Services.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

using PictureResult = Locompro.Models.Results.GetPicturesResult;

/// <summary>
///     Repository for access to Pictures in the database.
/// </summary>
public class PictureRepository : CrudRepository<Picture, PictureKey>, IPictureRepository
{
    public PictureRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }

    /// <summary>
    ///     Gets the pictures for a given item.
    /// </summary>
    /// <param name="pictureAmount"> Max amount of pictures that it is wished to be retrieved</param>
    /// <param name="productId"> Id of the product that has the pictures</param>
    /// <param name="storeName"> Name of the store where the product is located</param>
    /// <returns> A list of pictures </returns>
    public async Task<List<GetPicturesResult>> GetPicturesByItem(int pictureAmount, int productId, string storeName)
    {
        LocomproContext context = (LocomproContext) Context;
        
        IQueryable<GetPicturesResult> picturesQueryable =
            from p in context.GetPictures(storeName, productId, pictureAmount)
            select p;
        
        return await picturesQueryable.ToListAsync();
    }
}