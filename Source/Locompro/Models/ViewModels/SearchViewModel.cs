namespace Locompro.Models.ViewModels;

public class SearchViewModel
{
    public string ProductName { get; set; }
    public string ProvinceSelected { get; set; }
    public string CantonSelected { get; set; }
    public long MinPrice { get; set; }
    public long MaxPrice { get; set; }
    public string ModelSelected { get; set; }
    public string BrandSelected { get; set; }
    public string CategorySelected { get; set; }
}