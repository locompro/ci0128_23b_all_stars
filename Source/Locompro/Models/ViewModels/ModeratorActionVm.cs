using Locompro.Common;

namespace Locompro.Models.ViewModels;

public class ModeratorActionVm
{
    public ModeratorActions Action { get; set; }
    
    public string SubmissionUserId { get; set; }
    
    public DateTime SubmissionEntryTime { get; set; }
}