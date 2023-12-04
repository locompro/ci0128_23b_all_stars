using Locompro.Models.Dtos;

namespace Locompro.Services;

public interface IShoppingListService
{
    /// <summary>
    /// Returns all the products of the shopping list of an user
    /// </summary>
    /// <returns> Dto with list of products </returns>
    public Task<ShoppingListDto> Get();
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<ShoppingListSummaryDto> GetStoreSummary();
    
    /// <summary>
    /// Adds a product to the shopping list of an user
    /// </summary>
    /// <param name="productId"> ID of the product to be added </param>
    /// <returns></returns>
    public Task AddProduct(int productId);
    
    /// <summary>
    /// Removes a product from the shopping list of an user
    /// </summary>
    /// <param name="productId"> The ID of the product to be removed </param>
    /// <returns></returns>
    public Task DeleteProduct(int productId);
}