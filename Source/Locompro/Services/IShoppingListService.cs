using Locompro.Models.Entities;

namespace Locompro.Services
{
    public interface IShoppingListService
    {
        Task AddToShoppingList(int productId);
        Task RemoveFromShoppingList(int productId);
        Task<ShoppingList> GetShoppingList();
    }
}
