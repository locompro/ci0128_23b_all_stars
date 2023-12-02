using Locompro.Data;
using Locompro.Models.Entities;
using Locompro.Services.Auth;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Locompro.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly LocomproContext _dbContext;
        private readonly IAuthService _authService;

        public ShoppingListService(LocomproContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task AddToShoppingList(int productId)
        {
            var userId = _authService.GetUserId();
            var shoppingList = await _dbContext.ShoppingLists
                .Include(sl => sl.ShoppingListProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(sl => sl.UserId == userId);

            if (shoppingList == null)
            {
                shoppingList = new ShoppingList
                {
                    UserId = userId,
                    ShoppingListProducts = new List<ShoppingListProduct>()
                };
                _dbContext.ShoppingLists.Add(shoppingList);
            }

            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                var shoppingListProduct = new ShoppingListProduct
                {
                    ShoppingList = shoppingList,
                    Product = product
                };
                shoppingList.ShoppingListProducts.Add(shoppingListProduct);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveFromShoppingList(int productId)
        {
            var userId = _authService.GetUserId();
            var shoppingList = await _dbContext.ShoppingLists
                .Include(sl => sl.ShoppingListProducts)
                .FirstOrDefaultAsync(sl => sl.UserId == userId);

            if (shoppingList != null)
            {
                var shoppingListProduct = shoppingList.ShoppingListProducts.FirstOrDefault(sp => sp.ProductId == productId);
                if (shoppingListProduct != null)
                {
                    shoppingList.ShoppingListProducts.Remove(shoppingListProduct);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<ShoppingList> GetShoppingList()
        {
            var userId = _authService.GetUserId();
            return await _dbContext.ShoppingLists
                .Include(sl => sl.ShoppingListProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(sl => sl.UserId == userId) ?? new ShoppingList { UserId = userId, ShoppingListProducts = new List<ShoppingListProduct>() };
        }
    }
}
