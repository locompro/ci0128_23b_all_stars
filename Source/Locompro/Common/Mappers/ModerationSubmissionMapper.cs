using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ModerationSubmissionMapper : GenericMapper<SubmissionDto, List<ModerationSubmissionVm>>
{
    protected override List<ModerationSubmissionVm> BuildVm(SubmissionDto dto)
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

    protected override SubmissionDto BuildDto(List<ModerationSubmissionVm> vm)
    {
        return null;
    }
    
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