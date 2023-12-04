using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.Factories;

public class AutoReportFactory : GenericEntityFactory<AutoReportDto, AutoReport>
{
    protected override AutoReport BuildEntity(AutoReportDto dto)
    {
        return new AutoReport
        {
            SubmissionUserId = dto.SubmissionUserId,
            SubmissionEntryTime = dto.SubmissionEntryTime,
            UserId = dto.UserId,
            Description = dto.Description,
            Confidence = dto.Confidence,
            MinimumPrice = dto.MinimumPrice,
            MaximumPrice = dto.MaximumPrice,
            AveragePrice = dto.AveragePrice
        };
    }

    protected override AutoReportDto BuildDto(AutoReport entity)
    {
        return new AutoReportDto
        {
            SubmissionUserId = entity.SubmissionUserId,
            SubmissionEntryTime = entity.SubmissionEntryTime,
            UserId = entity.UserId,
            Description = entity.Description,
            Confidence = entity.Confidence,
            MinimumPrice = entity.MinimumPrice,
            MaximumPrice = entity.MaximumPrice,
            AveragePrice = entity.AveragePrice
        };
    }
}