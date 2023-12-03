using Locompro.Models.Entities;
namespace Locompro.Models.ViewModels
{
    public class ShoppingListVm
    {
        public int ShoppingListId { get; set; }

        public string UserId { get; set; }

        // You can include additional properties or logic specific to the view here

        public List<ProductVm> Products { get; set; }

        public ShoppingListVm(ShoppingList shoppingList)
        {
            ShoppingListId = shoppingList.ShoppingListId;
            UserId = shoppingList.UserId;

            // Populate Products from ShoppingListProducts
            Products = shoppingList.ShoppingListProducts
                .Select(slProduct => new ProductVm
                {
                    Id = slProduct.ProductId,
                    PName = slProduct.Product.Name,
                    Model = slProduct.Product.Model,
                    Brand = slProduct.Product.Brand,
                })
                .ToList();
        }
    }
}
