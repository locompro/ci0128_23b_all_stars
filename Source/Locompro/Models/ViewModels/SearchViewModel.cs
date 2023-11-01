namespace Locompro.Models.ViewModels;

public class SearchViewModel
{
    public string productName { get; set; }
    public string country { get; set; }
    public string province { get; set; }
    public string canton { get; set; }
    public int minValue { get; set; }
    public int maxValue { get; set; }
    public string category { get; set; }
    public string model { get; set; }
}