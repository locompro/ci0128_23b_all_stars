using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ShoppingListMapper : GenericMapper<ShoppingListDto, ShoppingListVm>
{
    protected override ShoppingListVm BuildVm(ShoppingListDto dto)
    {
        ShoppingListVm vm = new ShoppingListVm
        {
            UserId = dto.UserId,
            Products = dto.Products.Select(p => new ShoppingListProductVm()
            {
                Id = p.Id,
                Name = p.Name,
                Model = p.Model,
                Brand = p.Brand,
                MinPrice = p.MinPrice,
                MaxPrice = p.MaxPrice,
                TotalSubmissions = p.TotalSubmissions
            }).ToList()
        };

        return vm;
    }

    protected override ShoppingListDto BuildDto(ShoppingListVm vm)
    {
        ShoppingListDto dto = new ShoppingListDto
        {
            UserId = vm.UserId,
            Products = vm.Products.Select(p => new ShoppingListProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Model = p.Model,
                Brand = p.Brand,
                MinPrice = p.MinPrice,
                MaxPrice = p.MaxPrice,
                TotalSubmissions = p.TotalSubmissions
            }).ToList()
        };

        return dto;
    }
}