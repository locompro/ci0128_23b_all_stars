using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

/// <summary>
/// Repository for access to Pictures in the database.
/// </summary>
public class PictureRepository : CrudRepository<Picture, PictureKey>, IPictureRepository
{
    public PictureRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
    
    /// <summary>
    /// Gets the pictures for a given item.
    /// </summary>
    /// <param name="pictureAmount"> Max amount of pictures that it is wished to be retrieved</param>
    /// <param name="productName"> Name of the product that has the pictures</param>
    /// <param name="storeName"> Name of the store where the product is located</param>
    /// <returns> A list of pictures </returns>
    public async Task<List<Picture>> GetPicturesByItem(int pictureAmount, string productName, string storeName)
    {
        IQueryable<Picture> pictures = Set
            .Include(picture => picture.Submission)
            .ThenInclude(submission => submission.Product)
            .Where(picture => picture.Submission.Product.Name == productName)
            .Where(picture => picture.Submission.Store.Name == storeName);
        
        pictures = pictures.Take(pictureAmount);
        
        return await pictures.ToListAsync();
    }
}