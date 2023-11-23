using Locompro.Common;

namespace Locompro.Models.Dtos;

/// <summary>
/// Represents a data transfer object for moderator actions.
/// </summary>
public class ModeratorActionDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the moderator.
    /// This property holds the ID of the moderator who performed the action.
    /// </summary>
    public string ModeratorId { get; set; }
    
    /// <summary>
    /// Gets or sets the action performed by the moderator.
    /// This property is an enumeration of the various actions a moderator can perform, 
    /// such as approve, reject, or escalate a submission.
    /// </summary>
    public ModeratorActions Action { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier of the user who made the submission.
    /// This property holds the ID of the user whose submission is the subject of the moderator's action.
    /// </summary>
    public string SubmissionUserId { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the submission was made.
    /// This property records the exact time of the user's submission, 
    /// which can be used for tracking and auditing purposes.
    /// </summary>
    public DateTime SubmissionEntryTime { get; set; }
}