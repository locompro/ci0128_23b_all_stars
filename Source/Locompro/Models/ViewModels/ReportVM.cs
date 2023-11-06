using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

public class ReportVm
{
    [Required] public string SubmissionUserId { get; set; }

    [Required] public DateTime SubmissionEntryTime { get; set; }

    [Required] public string UserId { get; set; }
    
    public string UserName { get; set; }

    public string Description { get; set; }
}