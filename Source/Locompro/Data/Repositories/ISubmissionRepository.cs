using Locompro.Models;

namespace Locompro.Data.Repositories;

public interface ISubmissionRepository : ICrudRepository<Submission, SubmissionKey>
{
    /// <summary>
    /// Gets all submissions that are in a store in the given canton and province
    /// <param name="cantonName"></param>
    /// <param name="provinceName"></param>
    /// <returns> a task IEnumerable of submissions </returns>
    Task<IEnumerable<Submission>> GetByCantonAsync(string cantonName,
        string provinceName);

    /// <summary>
    /// Gets all submissions that contain the given product model
    /// </summary>
    /// <param name="productModel"></param>
    /// <returns> a task IEnumerable of submissions that contain the model</returns>
    Task<IEnumerable<Submission>> GetByProductModelAsync(string productModel);

    /// <summary>
    /// Gets all submissions that contain the given product name
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetByProductNameAsync(string productName);

    /// <summary>
    /// Gets all submissions that contain the given brand name, case insensitive
    /// </summary>
    /// <param name="brandName"></param>
    Task<IEnumerable<Submission>> GetByBrandAsync(string brandName);
}