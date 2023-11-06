using Locompro.Models.Entities;

namespace Locompro.Models.Dtos;

public class SubmissionDto
{
    public Func<IEnumerable<Submission>, Submission> BestSubmissionQualifier;
    
    public IEnumerable<Submission> Submissions { get; set; }
    
    public SubmissionDto(
        IEnumerable<Submission> submissions,
        Func<IEnumerable<Submission>, Submission> bestSubmissionQualifier)
    {
        Submissions = submissions;
        BestSubmissionQualifier = bestSubmissionQualifier;
    }
}