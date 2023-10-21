using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Domain service for Product entities.
    /// </summary>
    public class ProductService : AbstractDomainService<Product, int, ProductRepository>
    {
        /// <summary>
        /// Constructs a Product service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public ProductService(UnitOfWork unitOfWork, ProductRepository repository, ILoggerFactory loggerFactory) 
            : base(unitOfWork, repository, loggerFactory)
        {
        }
        
        public async Task<IEnumerable<Product>> GetByPartialName(string partialName)
        {
            await UnitOfWork.BeginTransaction();

            try
            {
                return await Repository.GetByPartialNameAsync(partialName);
            }
            catch (Exception)
            {
                await UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                await UnitOfWork.Commit();
            }
        }
    }
}
