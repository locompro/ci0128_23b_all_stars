using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ReportMapper : GenericMapper<ReportDto, UserReportVm>
{
    protected override UserReportVm BuildVm(ReportDto dto)
    {
        return new UserReportVm
        {
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime,
            UserId = dto.UserId,
            Description = dto.Description
        };
    }

    protected override ReportDto BuildDto(UserReportVm vm)
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