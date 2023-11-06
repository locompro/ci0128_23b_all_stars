using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.Entities;

public class Report
{
    [Required] public string SubmissionUserId { get; set; }

    [Required] public DateTime SubmissionEntryTime { get; set; }

    [Required] public string UserId { get; set; }

    public string Description { get; set; }

    public virtual Submission Submission { get; set; }

    public virtual User User { get; set; }
}