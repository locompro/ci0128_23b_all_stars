namespace Locompro.Models.ViewModels;

public class UserReportedSubmissionVm
{
    public string UserId { get; set; }

    public DateTime EntryTime { get; set; }

    public string Author { get; set; }

    public string Product { get; set; }

    public double Price { get; set; }

    public string Store { get; set; }

    public string Model { get; set; }

    public string Province { get; set; }

    public string Canton { get; set; }

    public string Description { get; set; }

    public List<UserReportVm> Reports { get; set; }
}