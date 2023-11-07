namespace Locompro.Models.Dtos;

public class ReportDto
{
    public string SubmissionUserId { get; set; }

    public DateTime SubmissionEntryTime { get; set; }

    public string UserId { get; set; }
    
    public string UserName { get; set; }

    public string Description { get; set; }
    
    public override string ToString()
    {
        return $"ReportDto: SubmissionUserId={SubmissionUserId}, " +
               $"SubmissionEntryTime={SubmissionEntryTime}, " +
               $"UserId={UserId}";
    }
}