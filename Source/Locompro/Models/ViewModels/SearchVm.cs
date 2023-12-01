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
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public long Distance { get; set; }
    public string MapGeneratedAddress { get; set; }

    public bool IsEmpty() =>
            string.IsNullOrEmpty(ProductName) &&
            string.IsNullOrEmpty(ProvinceSelected) &&
            string.IsNullOrEmpty(CantonSelected) &&
            string.IsNullOrEmpty(ModelSelected) &&
            string.IsNullOrEmpty(BrandSelected) &&
            string.IsNullOrEmpty(CategorySelected) &&
            MinPrice == 0 && MaxPrice == 0 &&
            Latitude == 0 && Longitude == 0 &&
            string.IsNullOrEmpty(MapGeneratedAddress) &&
            Distance == 0;
}