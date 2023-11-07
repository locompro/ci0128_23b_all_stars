namespace Locompro.Models.ViewModels;

public class SearchVm
{
    public string ProductName { get; set; }
    public string ProvinceSelected { get; set; }
    public string CantonSelected { get; set; }
    public long MinPrice { get; set; }
    public long MaxPrice { get; set; }
    public string ModelSelected { get; set; }
    public string BrandSelected { get; set; }
    public string CategorySelected { get; set; }
    public int ResultsPerPage { get; set; }

    public bool IsEmpty() =>
            string.IsNullOrEmpty(ProductName) &&
            string.IsNullOrEmpty(ProvinceSelected) &&
            string.IsNullOrEmpty(CantonSelected) &&
            string.IsNullOrEmpty(ModelSelected) &&
            string.IsNullOrEmpty(BrandSelected) &&
            string.IsNullOrEmpty(CategorySelected) &&
            MinPrice == 0 && MaxPrice == 0;
}