namespace Locompro.Models.Dtos;

public class ProductDto
{
    public string UserId { get; set; }
    
    public List<ShoppingListProductDto> Products { get; set; }
}