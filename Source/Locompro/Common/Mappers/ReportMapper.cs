using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ReportMapper : GenericMapper<ReportDto, ReportVm>
{
    protected override ReportVm BuildVm(ReportDto dto)
    {
        return new ReportVm
        {
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime,
            UserId = dto.UserId,
            Description = dto.Description
        };
    }

    protected override ReportDto BuildDto(ReportVm vm)
    {
        return new ReportDto
        {
            SubmissionUserId = vm.SubmissionUserId,
            SubmissionEntryTime = vm.SubmissionEntryTime,
            UserId = vm.UserId,
            Description = vm.Description
        };
    }
}