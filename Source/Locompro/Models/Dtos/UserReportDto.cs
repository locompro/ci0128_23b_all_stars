namespace Locompro.Models.Dtos;

/// <summary>
/// Represents a data transfer object for reports.
/// </summary>
public class UserReportDto
{
    public string SubmissionUserId { get; set; }

    /// <summary>
    /// Gets or sets the time at which the submission was entered.
    /// </summary>
    /// <value>The entry time of the submission.</value>
    public DateTime SubmissionEntryTime { get; set; }

    /// <summary>
    /// Gets or sets the user ID of the individual who made the report.
    /// </summary>
    /// <value>The user ID of the reporter.</value>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the individual who made the report.
    /// </summary>
    /// <value>The username of the reporter.</value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the description of the report.
    /// </summary>
    /// <value>The description of the report.</value>
    public string Description { get; set; }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current <see cref="UserReportDto"/>.</returns>
    public override string ToString()
    {
        return $"UserReportDto: SubmissionUserId={SubmissionUserId}, " +
               $"SubmissionEntryTime={SubmissionEntryTime}, " +
               $"UserId={UserId}";
    }
}