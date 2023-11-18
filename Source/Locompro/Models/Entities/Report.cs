using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.Entities;

/// <summary>
/// Represents a report made by a user against a submission.
/// </summary>
public class Report
{
    /// <summary>
    /// Gets or sets the user ID of the individual who made the submission being reported.
    /// </summary>
    /// <value>The user ID of the submitter.</value>
    [Required]
    public string SubmissionUserId { get; set; }

    /// <summary>
    /// Gets or sets the time at which the reported submission was entered.
    /// </summary>
    /// <value>The entry time of the reported submission.</value>
    [Required]
    public DateTime SubmissionEntryTime { get; set; }

    /// <summary>
    /// Gets or sets the user ID of the individual who made the report.
    /// </summary>
    /// <value>The user ID of the reporter.</value>
    [Required]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the description of the report, detailing the reasons for the report.
    /// </summary>
    /// <value>The description of the report.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the submission that is associated with this report.
    /// </summary>
    /// <value>The submission being reported.</value>
    public virtual Submission Submission { get; set; }

    /// <summary>
    /// Gets or sets the user who reported the submission.
    /// </summary>
    /// <value>The user who made the report.</value>
    public virtual User User { get; set; }
}
