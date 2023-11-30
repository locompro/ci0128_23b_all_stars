namespace Locompro.Models.ViewModels;

public class AutoReportVm
{
    public string Product { get; set; }
    public string Store { get; set; }
    public string Description { get; set; }
    public float Confidence { get; set; }
    public int Price { get; set; }
    public int MinimumPrice { get; set; }
    public int MaximumPrice { get; set; }
    public float AveragePrice { get; set; }
}