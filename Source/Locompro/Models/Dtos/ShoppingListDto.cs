namespace Locompro.Models.Dtos;

// Hacer sólo un ShoppingListMapper que mapee también shopping list product DTO iterativamente
public class ShoppingListDto
{
    public string UserId;

    public List<ShoppingListProductDto> Products;
}