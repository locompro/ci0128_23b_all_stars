using Locompro.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locompro.Data.Repositories
{
    /// <summary>
    /// Defines the interface for a repository managing 'Product' entities with CRUD operations.
    /// </summary>
    public interface IProductRepository : ICrudRepository<Product, int>
    {
        /// <summary>
        /// Asynchronously retrieves a list of distinct brands from the 'Product' entities.
        /// </summary>
        /// <returns>The task result contains a list of brand names.</returns>
        Task<List<string>> GetBrandsAsync();

        /// <summary>
        /// Asynchronously retrieves a list of distinct models from the 'Product' entities.
        /// </summary>
        /// <returns>The task result contains a list of model names.</returns>
        Task<List<string>> GetModelsAsync();
    }
}