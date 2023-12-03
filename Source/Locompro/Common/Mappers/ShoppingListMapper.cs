using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ShoppingListMapper : GenericMapper<ShoppingListDto, ShoppingListVm>
{
    protected override ShoppingListVm BuildVm(ShoppingListDto dto)
    {
        throw new NotImplementedException();
    }

    protected override ShoppingListDto BuildDto(ShoppingListVm vm)
    {
        throw new NotImplementedException();
    }
}