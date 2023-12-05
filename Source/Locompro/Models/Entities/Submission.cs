using Microsoft.Build.Framework;

namespace Locompro.Models.Entities;

/// <summary>
///     A price submission by a user.
/// </summary>
public class Submission
{
    [Required] public string UserId { get; set; }

    [Required] public DateTime EntryTime { get; set; }

    [Required] public SubmissionStatus Status { get; set; } = SubmissionStatus.New;

    [Required] public int Price { get; set; }

    public float Rating { get; set; }

    public string Description { get; set; }

    [Required] public string StoreName { get; set; }

    [Required] public int ProductId { get; set; }

    public virtual User User { get; set; }

    public virtual Store Store { get; set; }

    public virtual Product Product { get; set; }

    public virtual ICollection<Picture> Pictures { get; set; }

    public virtual ICollection<UserReport> UserReports { get; set; }

    public virtual ICollection<AutoReport> AutoReports { get; set; }

    public long NumberOfRatings { get; set; }

    public virtual ICollection<User> Approvers { get; set; }

    public virtual ICollection<User> Rejecters { get; set; }
}