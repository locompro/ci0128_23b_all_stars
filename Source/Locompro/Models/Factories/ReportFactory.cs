using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.Factories;

/// <summary>
/// A factory for converting between <see cref="ReportDto"/> and <see cref="Report"/> objects.
/// </summary>
public class ReportFactory : GenericEntityFactory<ReportDto, Report>
{
    /// <inheritdoc />
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

    /// <inheritdoc />
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
