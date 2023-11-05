using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

/// <summary>
/// Key to find an picture in the database by its key
/// </summary>
public class PictureKey
{
    public string SubmissionUserId { get; set; }
    public DateTime SubmissionEntryTime { get; set; }
    public int Id { get; set; }
}

/// <summary>
/// Service for handling the access of pictures
/// </summary>
public class PictureService : DomainService<Picture, PictureKey>, IPictureService
{
    private readonly IPictureRepository _pictureRepository;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="loggerFactory"></param>
    public PictureService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory)
    {
        _pictureRepository = unitOfWork.GetSpecialRepository<IPictureRepository>();
    }

    /// <summary>
    /// Returns the given pictures for an item described by the product name and the store name
    /// </summary>
    /// <param name="pictureAmount"> Max amount of pictures that it is wished to be retrieved</param>
    /// <param name="productName"> Name of the product that has the pictures</param>
    /// <param name="storeName"> Name of the store where the product is located</param>
    /// <returns> A list of pictures </returns>
    public async Task<List<Picture>> GetPicturesForItem(int pictureAmount, string productName, string storeName)
    {
        return await _pictureRepository.GetPicturesByItem(pictureAmount, productName, storeName);
    }
}