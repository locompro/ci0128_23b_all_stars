namespace Locompro.Models.Dtos;

public class ShoppingListSummaryStoreDto
{
    public string Name { get; set; }
    
    public string Province { get; set; }
    
    public string Canton { get; set; }

    public int ProductsAvailable { get; set; }
    
    public float PercentageProductsAvailable { get; set; }
    
    public int TotalCost { get; set; }
}