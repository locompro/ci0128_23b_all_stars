using Locompro.Models.Dtos;

namespace Locompro.Services;

public interface IShoppingListService
{
    public Task<ShoppingListDto> Get();

    public Task<StoreSummaryDto> GetStoreSummary();

    public Task AddProduct(int productId);
    
    public Task DeleteProduct(int productId);
}