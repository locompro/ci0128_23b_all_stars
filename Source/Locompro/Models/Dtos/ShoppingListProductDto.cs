namespace Locompro.Models.Dtos;

public class ShoppingListProductDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Model { get; set; }
    
    public string Brand { get; set; }
    
    public int MinPrice { get; set; }
    
    public int MaxPrice { get; set; }
    
    public int TotalSubmissions { get; set; }
}