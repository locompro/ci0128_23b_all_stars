namespace Locompro.Models.ViewModels;

public class ShoppingListSummaryVm
{
    public string UserId { get; set; }

    public List<ShoppingListSummaryStoreVm> Stores { get; set; }
}