namespace Locompro.Models;

public class DisplaySubmission
{
    public string DateTime { get;}
    
    public string Price { get;}
    
    public string Description { get;}

    public DisplaySubmission(Submission submission, Func<Submission, string> getFormattedDate)
    {
        this.DateTime = getFormattedDate(submission);
        this.Price = submission.Price.ToString();
        this.Description = submission.Description;
    }
}