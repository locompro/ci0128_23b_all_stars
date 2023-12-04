namespace Locompro.Models.Dtos;

public class ShoppingListSummaryDto
{
    public string UserId { get; set; }

    public List<ShoppingListSummaryStoreDto> Stores { get; set; }
}