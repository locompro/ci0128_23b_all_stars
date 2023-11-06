namespace Locompro.Services.Domain
{
    /// <summary>
    /// Provides services for managing product-related operations.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Asynchronously retrieves a list of all distinct product brands.
        /// </summary>
        /// <returns>The task result contains a list of brand names as strings.</returns>
        Task<List<string>> GetBrandsAsync();

        /// <summary>
        /// Asynchronously retrieves a list of all distinct product models.
        /// </summary>
        /// <returns>The task result contains a list of model names as strings.</returns>
        Task<List<string>> GetModelsAsync();
    }
}