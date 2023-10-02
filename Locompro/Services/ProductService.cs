using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Locompro.Services;

namespace Locompro.Services
{
    public class ProductService : AbstractDomainService<Product, string, ProductRepository>
    {
        public ProductService(UnitOfWork unitOfWork, ProductRepository productRepository)
            : base(unitOfWork, productRepository)
        {
        }
    }
}
