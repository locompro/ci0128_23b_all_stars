namespace Locompro.Models.Entities
{
    public class ShoppingListProduct
    {
        public int ShoppingListId { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
