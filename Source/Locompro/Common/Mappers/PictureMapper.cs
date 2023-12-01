using Locompro.Models.Dtos;
using Locompro.Models.Results;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class PictureMapper : GenericMapper<PictureDto, PictureVm>
{
    protected override PictureVm BuildVm(PictureDto dto)
    {
        return new PictureVm
        {
            Name = dto.Name,
            PictureData = dto.PictureData
        };
    }

    protected override PictureDto BuildDto(PictureVm vm)
    {
        return new PictureDto
        {
            Name = vm.Name,
            PictureData = vm.PictureData
        };
    }
}