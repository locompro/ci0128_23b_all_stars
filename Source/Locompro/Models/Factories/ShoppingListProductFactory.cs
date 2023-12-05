using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.Factories;

public class ShoppingListProductFactory : GenericEntityFactory<ShoppingListProductDto, Product>
{
    protected override Product BuildEntity(ShoppingListProductDto dto)
    {
        return new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Model = dto.Model,
            Brand = dto.Brand
        };
    }

    protected override ShoppingListProductDto BuildDto(Product entity)
    {
        return new ShoppingListProductDto
        {
            Id = entity.Id,
            Name = string.IsNullOrEmpty(entity.Name) ? "" : entity.Name,
            Model =  string.IsNullOrEmpty(entity.Model) ? "" : entity.Model,
            Brand = string.IsNullOrEmpty(entity.Brand) ? "" : entity.Brand,
            MinPrice = entity.Submissions?.Min(s => s.Price) ?? -1,
            MaxPrice = entity.Submissions?.Max(s => s.Price) ?? -1,
            TotalSubmissions = entity.Submissions?.Count ?? 0
        };
    }
}