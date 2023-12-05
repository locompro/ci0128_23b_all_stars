using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Models.ViewModels;

public class UserReportVm
{
    [Required] public string SubmissionUserId { get; set; }

    [Required] public DateTime SubmissionEntryTime { get; set; }

    [Required] public string UserId { get; set; }

    public string UserName { get; set; }

    [StringLength(140)] public string Description { get; set; }

    public override string ToString()
    {
        return $"UserReportVm: SubmissionUserId={SubmissionUserId}, " +
               $"SubmissionEntryTime={SubmissionEntryTime}, " +
               $"UserId={UserId}";
    }
}