namespace Locompro.Services.Tasks;

/// <summary>
/// Scheduled task for finding price anomalies.
/// This task uses the <see cref="IAnomalyDetectionService"/> to detect and handle anomalies in submission prices.
/// It is scheduled to run at a regular interval, currently set to once every day.
/// </summary>
public class FindPriceAnomaliesTask : ScheduledTaskBase<IAnomalyDetectionService>
{
    /// <summary>
    /// The interval at which the task should run. Currently set to run every 1 day.
    /// </summary>
    private static readonly TimeSpan _interval = TimeSpan.FromDays(1);

    /// <summary>
    /// Initializes a new instance of the <see cref="FindPriceAnomaliesTask"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve services.</param>
    public FindPriceAnomaliesTask(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Interval = _interval;
    }

    /// <summary>
    /// Executes the task to find price anomalies.
    /// This method is called according to the specified interval and will run the anomaly detection process.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override Task ExecuteScopedAsync(CancellationToken cancellationToken)
    {
        return ScopedService.FindPriceAnomaliesAsync();
    }
}