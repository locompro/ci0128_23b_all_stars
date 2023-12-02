namespace Locompro.Models.Entities;

public class AutoReport : Report
{
    /// <summary>
    /// The confidence of the price anomaly
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// The minimum price of the product in the store
    /// </summary>
    public int MinimumPrice { get; set; }

    /// <summary>
    /// The maximum price of the product in the store
    /// </summary>
    public int MaximumPrice { get; set; }

    /// <summary>
    /// The average price, used to compare with the price of the product in the store
    /// </summary>
    public double AveragePrice { get; set; }
}