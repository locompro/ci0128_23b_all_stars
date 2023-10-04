using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Locompro.Services;
using Microsoft.Extensions.Logging;

namespace Locompro.Services
{
    /// <summary>
    /// Service for Product
    /// </summary>
    public class ProductService : AbstractDomainService<Product, string, ProductRepository>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="productRepository"></param>
        /// <param name="loggerFactory"></param>
        public ProductService(UnitOfWork unitOfWork, ProductRepository productRepository, ILoggerFactory loggerFactory)
            : base(unitOfWork, productRepository, loggerFactory)
        {
        }
    }
}
