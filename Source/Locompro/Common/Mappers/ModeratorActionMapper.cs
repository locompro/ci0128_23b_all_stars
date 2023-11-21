using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ModeratorActionMapper : GenericMapper<ModeratorActionDto, ModeratorActionVm>
{
    protected override ModeratorActionVm BuildVm(ModeratorActionDto dto)
    {
        return new ModeratorActionVm
        {
            Action = dto.Action,
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime
        };
    }

    protected override ModeratorActionDto BuildDto(ModeratorActionVm vm)
    {
        return new ModeratorActionDto
        {
            Action = vm.Action,
            SubmissionUserId = vm.SubmissionUserId,
            SubmissionEntryTime = vm.SubmissionEntryTime
        };
    }
}