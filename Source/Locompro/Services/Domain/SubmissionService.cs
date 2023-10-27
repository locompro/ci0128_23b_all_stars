using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

public class SubmissionService : DomainService<Submission, SubmissionKey>, ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    
    public SubmissionService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _submissionRepository = UnitOfWork.GetRepository<ISubmissionRepository>();
    }
    
    /// <summary>
    /// Gets submissions containing a specific product name
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Submission>> GetByProductName(string productName)
    {
        return await _submissionRepository.GetByProductNameAsync(productName);
    }

    /// <summary>
    /// Gets submissions containing a specific product model
    /// </summary>
    /// <remarks> This is just a wrapper for the submission repository </remarks>
    public async Task<IEnumerable<Submission>> GetByProductModel(string productModel)
    {
        return await _submissionRepository.GetByProductModelAsync(productModel);
    }

    /// <summary>
    /// Calls the submission repository to get all submissions containing a specific brand name
    /// </summary>
    /// <param name="brandName"></param>
    /// <returns> An Enumerable with al the submissions tha meet the criteria</returns>
    public async Task<IEnumerable<Submission>> GetByBrand(string brandName)
    {
        return await _submissionRepository.GetByBrandAsync(brandName);
    }

    public async Task<IEnumerable<Submission>> GetByCantonAndProvince(string canton, string province)
    {
        return await _submissionRepository.GetByCantonAsync(canton, province);
    }

    public async Task<IEnumerable<Submission>> GetByCanton(string canton, string province)
    {
        return await _submissionRepository.GetByCantonAsync(canton, province);
    }
}