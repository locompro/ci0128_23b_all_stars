using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

public class ProductService : DomainService<Product, int>, IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _productRepository = UnitOfWork.GetSpecialRepository<IProductRepository>();
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetBrandsAsync()
    {
        return await _productRepository.GetBrandsAsync();
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetModelsAsync()
    {
        return await _productRepository.GetModelsAsync();
    }
}