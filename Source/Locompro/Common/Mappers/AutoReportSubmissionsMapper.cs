using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class AutoReportSubmissionsMapper : GenericMapper<SubmissionsDto, List<AutoReportVm>>
{
    /// <summary>
    /// Builds a list of AutoReportVms from a SubmissionsDto
    /// </summary>
    /// <param name="dto"> The dto to map from</param>
    /// <returns>A list of AutoReportVm</returns>
    protected override List<AutoReportVm> BuildVm(SubmissionsDto dto)
    {
        // Create a list of AutoReportVms
        var autoReportVms = new List<AutoReportVm>();
        // If the dto is null, return an empty list
        if (dto?.Submissions == null) return autoReportVms;
        // For each submission in the dto, map every attribute to its corresponding vm attribute
        autoReportVms.AddRange(dto.Submissions.Select(GetAutoReportVm));

        // Return the list of AutoReportVms
        return autoReportVms;
    }

    protected override SubmissionsDto BuildDto(List<AutoReportVm> vm)
    {
        return null;
    }

    /// <summary>
    /// Maps a submission to an AutoReportVm
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private static AutoReportVm GetAutoReportVm(Submission submission)
    {
        return new AutoReportVm
        {
            SubmissionUserId = submission.UserId,
            SubmissionEntryTime = submission.EntryTime,
            Price = submission.Price,
            Product = submission.Product.Name,
            Store = submission.Store.Name,
            AveragePrice = submission.AutoReports.Last().AveragePrice,
            MinimumPrice = submission.AutoReports.Last().MinimumPrice,
            MaximumPrice = submission.AutoReports.Last().MaximumPrice,
            Confidence = submission.AutoReports.Last().Confidence,
            Description = submission.Description
        };
    }
}