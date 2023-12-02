using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.Factories;

/// <summary>
/// A factory for converting between <see cref="UserReportDto"/> and <see cref="Report"/> objects.
/// </summary>
public class ReportFactory : GenericEntityFactory<UserReportDto, UserReport>
{
    /// <inheritdoc />
    protected override UserReport BuildEntity(UserReportDto dto)
    {
        return new UserReport
        {
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime,
            UserId = dto.UserId,
            Description = dto.Description
        };
    }

    /// <inheritdoc />
    protected override UserReportDto BuildDto(UserReport entity)
    {
        return new UserReportDto
        {
            SubmissionUserId = entity.SubmissionUserId,
            SubmissionEntryTime = entity.SubmissionEntryTime,
            UserId = entity.UserId,
            Description = entity.Description
        };
    }
}