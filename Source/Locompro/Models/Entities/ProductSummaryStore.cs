namespace Locompro.Models.Entities;

public class ProductSummaryStore
{
    public string Name { get; set; }
    
    public Province Province { get; set; }
    
    public Canton Canton { get; set; }

    public int ProductsAvailable { get; set; }
    
    public int PercentageProductsAvailable { get; set; }
    
    public int TotalCost { get; set; }
}