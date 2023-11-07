using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

/// <summary>
/// Mapper than takes a Submission DTO and maps it to a list of ModerationSubmissionViewModels
/// Each submission has a number of reports, which are mapped to a list of ReportViewModels
/// </summary>
public class ModerationSubmissionsMapper : GenericMapper<SubmissionsDto, List<ModerationSubmissionVm>>
{
    protected override List<ModerationSubmissionVm> BuildVm(SubmissionsDto dto)
    {
        List<ModerationSubmissionVm> moderationSubmissionViewModels = new ();

        if (dto == null || dto.Submissions == null || !dto.Submissions.Any() || dto.BestSubmissionQualifier == null)
        {
            return moderationSubmissionViewModels;
        }
        
        foreach (Submission submission in dto.Submissions)
        {
            List<ReportVm> reports = GetReportVmFromReports(submission.Reports.ToList());
            
            ModerationSubmissionVm newModerationSubmissionVm = new()
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
    
    
    protected override SubmissionsDto BuildDto(List<ModerationSubmissionVm> vm)
    {
        return null;
    }
    
    /// <summary>
    /// Gets reportVMs mapped for this use case from a list of reports
    /// </summary>
    /// <param name="reports"> reports to be mapped </param>
    /// <returns> mapped report VMS </returns>
    private static List<ReportVm> GetReportVmFromReports(List<Report> reports)
    {
        if (reports == null || !reports.Any())
        {
            return new List<ReportVm>();
        }
        
        return reports.Select(report => new ReportVm
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