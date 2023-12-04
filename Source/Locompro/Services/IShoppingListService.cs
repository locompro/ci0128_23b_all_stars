using Locompro.Models.Dtos;

namespace Locompro.Services;

public interface IShoppingListService
{
    public Task<ShoppingListDto> Get();

    public Task<ShoppingListSummaryDto> GetStoreSummary();

    public Task AddProduct();
    
    public Task DeleteProduct();
}