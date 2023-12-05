using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.Factories;

public class ShoppingListSummaryStoreFactory : GenericEntityFactory<ShoppingListSummaryStoreDto, ProductSummaryStore>
{
    protected override ProductSummaryStore BuildEntity(ShoppingListSummaryStoreDto dto)
    {
        throw new NotImplementedException();
    }

    protected override ShoppingListSummaryStoreDto BuildDto(ProductSummaryStore entity)
    {
        return new ShoppingListSummaryStoreDto
        {
            Name = entity.Name,
            Province = entity.Province.Name,
            Canton = entity.Canton.Name,
            ProductsAvailable = entity.ProductsAvailable,
            PercentageProductsAvailable = entity.PercentageProductsAvailable,
            TotalCost = entity.TotalCost
        };
    }
}