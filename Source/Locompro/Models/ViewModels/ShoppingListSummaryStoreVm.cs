namespace Locompro.Models.ViewModels;

public class ShoppingListSummaryStoreVm
{
    public string Name { get; set; }
    
    public string Province { get; set; }
    
    public string Canton { get; set; }

    public int ProductsAvailable { get; set; }
    
    public int PercentageProductsAvailable { get; set; }
    
    public int TotalCost { get; set; }
}