using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.Factories;

public class ReportFactory : GenericEntityFactory<ReportDto, Report>
{
    protected override Report BuildEntity(ReportDto dto)
    {
        return new Report
        {
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime,
            UserId = dto.UserId,
            Description = dto.Description
        };
    }

    protected override ReportDto BuildDto(Report entity)
    {
        return new ReportDto
        {
            SubmissionUserId = entity.SubmissionUserId,
            SubmissionEntryTime = entity.SubmissionEntryTime,
            UserId = entity.UserId,
            Description = entity.Description
        };
    }
}