namespace Locompro.Models;

public class DisplaySubmission
{
    public string EntryTime { get;}
    
    public int Price { get;}
    
    public string Description { get;}

    public DisplaySubmission(Submission submission, Func<Submission, string> getFormattedDate)
    {
        this.EntryTime = getFormattedDate(submission);
        this.Price = submission.Price;
        this.Description = submission.Description;
    }
}