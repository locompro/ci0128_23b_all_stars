using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ShoppingListSummaryMapper : GenericMapper<ShoppingListSummaryDto, ShoppingListSummaryVm>
{
    protected override ShoppingListSummaryVm BuildVm(ShoppingListSummaryDto dto)
    {
        ShoppingListSummaryVm vm = new ShoppingListSummaryVm()
        {
            UserId = dto.UserId,
            Stores = dto.Stores.Select(s => new ShoppingListSummaryStoreVm()
            {
                Name = s.Name,
                Province = s.Province,
                Canton = s.Canton,
                ProductsAvailable = s.ProductsAvailable,
                PercentageProductsAvailable = s.PercentageProductsAvailable,
                TotalCost = s.TotalCost
            }).ToList()
        };

        return vm;
    }

    protected override ShoppingListSummaryDto BuildDto(ShoppingListSummaryVm vm)
    {
        ShoppingListSummaryDto dto = new ShoppingListSummaryDto()
        {
            UserId = vm.UserId,
            Stores = vm.Stores.Select(s => new ShoppingListSummaryStoreDto()
            {
                Name = s.Name,
                Province = s.Province,
                Canton = s.Canton,
                ProductsAvailable = s.ProductsAvailable,
                PercentageProductsAvailable = s.PercentageProductsAvailable,
                TotalCost = s.TotalCost
            }).ToList()
        };

        return dto;
    }
}