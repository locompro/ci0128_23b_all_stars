namespace Locompro.Models.ViewModels;

/// <summary>
///     View Model for updating the rating of a submission
///     Rating is a string for json parsing reasons
/// </summary>
public class RatingVm
{
    public string SubmissionUserId { get; set; }
    public DateTime SubmissionEntryTime { get; set; }
    public string Rating { get; set; }
}