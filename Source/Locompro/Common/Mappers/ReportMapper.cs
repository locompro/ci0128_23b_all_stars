using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ReportMapper : GenericMapper<UserReportDto, UserReportVm>
{
    protected override UserReportVm BuildVm(UserReportDto dto)
    {
        return new UserReportVm
        {
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime,
            UserId = dto.UserId,
            Description = dto.Description
        };
    }

    protected override UserReportDto BuildDto(UserReportVm vm)
    {
        return new UserReportDto
        {
            SubmissionUserId = vm.SubmissionUserId,
            SubmissionEntryTime = vm.SubmissionEntryTime,
            UserId = vm.UserId,
            Description = vm.Description
        };
    }
}