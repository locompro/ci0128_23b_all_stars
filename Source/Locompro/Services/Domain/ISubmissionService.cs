using Locompro.Data.Repositories;
using Locompro.Models;

namespace Locompro.Services.Domain;

public interface ISubmissionService : IDomainService<Submission, SubmissionKey>
{
    /// <summary>
    /// Gets submissions containing a specific product name
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetByProductName(string productName);

    /// <summary>
    /// Gets submissions containing a specific product model
    /// </summary>
    /// <remarks> This is just a wrapper for the submission repository </remarks>
    Task<IEnumerable<Submission>> GetByProductModel(string productModel);

    /// <summary>
    /// Calls the submission repository to get all submissions containing a specific brand name
    /// </summary>
    /// <param name="brandName"></param>
    /// <returns> An Enumerable with al the submissions tha meet the criteria</returns>
    Task<IEnumerable<Submission>> GetByBrand(string brandName);

    Task<IEnumerable<Submission>> GetByCantonAndProvince(string canton, string province);
    Task<IEnumerable<Submission>> GetByCanton(string canton, string province);
}