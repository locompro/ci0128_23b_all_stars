namespace Locompro.Models.ViewModels;

public class ModeratorActionOnReportVm
{
    public ModeratorActions Action { get; set; }
    
    public string SubmissionUserId { get; set; }
    
    public DateTime SubmissionEntryTime { get; set; }
}

public enum ModeratorActions
{
    Default,
    EraseSubmission,
    EraseReport
}