using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Models.Dtos;

public class SubmissionsDto
{
    public readonly Func<IEnumerable<Submission>, Submission> BestSubmissionQualifier;

    public readonly IEnumerable<Submission> Submissions;
    
    public SubmissionsDto(
        IEnumerable<Submission> submissions,
        Func<IEnumerable<Submission>, Submission> bestSubmissionQualifier)
    {
        Submissions = submissions;
        BestSubmissionQualifier = bestSubmissionQualifier;
    }
}