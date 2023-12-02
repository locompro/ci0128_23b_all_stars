namespace Locompro.Models.Entities;

public class UserReport : Report
{
    /// <summary>
    /// Gets or sets the description of the report, detailing the reasons for the report.
    /// </summary>
    /// <value>The description of the report.</value>
    public new string Description { get; set; }
}