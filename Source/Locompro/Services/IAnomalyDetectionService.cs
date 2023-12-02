namespace Locompro.Services;

public interface IAnomalyDetectionService
{
    /// <summary>
    /// Finds price anomalies in the database.
    /// </summary>
    /// <returns>An async operation</returns>
    Task FindPriceAnomaliesAsync();
}