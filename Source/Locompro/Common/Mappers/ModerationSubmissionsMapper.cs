using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

/// <summary>
/// Mapper than takes a Submission DTO and maps it to a list of ModerationSubmissionViewModels
/// Each submission has a number of reports, which are mapped to a list of ReportViewModels
/// </summary>
public class ModerationSubmissionsMapper : GenericMapper<SubmissionsDto, List<UserReportedSubmissionVm>>
{
    protected override List<UserReportedSubmissionVm> BuildVm(SubmissionsDto dto)
    {
        List<UserReportedSubmissionVm> moderationSubmissionViewModels = new();

        if (dto == null || dto.Submissions == null || !dto.Submissions.Any() || dto.BestSubmissionQualifier == null)
        {
            return moderationSubmissionViewModels;
        }

        foreach (Submission submission in dto.Submissions)
        {
            List<UserReportVm> reports = GetReportVmFromReports(submission.Reports.ToList());

            UserReportedSubmissionVm newModerationSubmissionVm = new()
            {
                UserId = submission.UserId,
                EntryTime = submission.EntryTime,
                Author = submission.User.UserName,
                Product = submission.Product.Name,
                Price = submission.Price,
                Store = submission.Store.Name,
                Model = submission.Product.Model,
                Province = submission.Store.Canton.Province.Name,
                Canton = submission.Store.Canton.Name,
                Description = submission.Description,
                Reports = reports
            };

            moderationSubmissionViewModels.Add(newModerationSubmissionVm);
        }

        return moderationSubmissionViewModels;
    }


    protected override SubmissionsDto BuildDto(List<UserReportedSubmissionVm> vm)
    {
        return null;
    }

    /// <summary>
    /// Gets reportVMs mapped for this use case from a list of reports
    /// </summary>
    /// <param name="reports"> reports to be mapped </param>
    /// <returns> mapped report VMS </returns>
    private static List<UserReportVm> GetReportVmFromReports(List<UserReport> reports)
    {
        if (reports == null || !reports.Any())
        {
            return new List<UserReportVm>();
        }

        return reports.Select(report => new UserReportVm
            {
                SubmissionUserId = report.SubmissionUserId,
                SubmissionEntryTime = report.SubmissionEntryTime,
                UserId = report.UserId,
                UserName = report.User.UserName,
                Description = report.Description
            })
            .ToList();
    }
}