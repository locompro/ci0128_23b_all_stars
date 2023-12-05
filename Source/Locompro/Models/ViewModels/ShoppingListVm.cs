namespace Locompro.Models.ViewModels;

public class ShoppingListVm
{
    public string UserId { get; set; }
    
    public List<ShoppingListProductVm> Products { get; set; }
}