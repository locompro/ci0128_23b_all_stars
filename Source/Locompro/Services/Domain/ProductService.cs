using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

public class ProductService : Service, IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(ILoggerFactory loggerFactory,
        IProductRepository productRepository) : base(loggerFactory)
    {
        _productRepository = productRepository;
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