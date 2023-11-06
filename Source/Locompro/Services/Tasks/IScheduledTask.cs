namespace Locompro.Services.Tasks;

/// <summary>
///     Interface for a task that runs in the background.
/// </summary>
public interface IScheduledTask
{
    /// <summary>
    ///     Identifies how often the task should run.
    ///     Format: [Days,] Hours, Minutes, Seconds
    /// </summary>
    TimeSpan Interval { get; }


    Task ExecuteAsync(CancellationToken cancellationToken);
}