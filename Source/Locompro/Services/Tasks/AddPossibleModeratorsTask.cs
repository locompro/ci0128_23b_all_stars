namespace Locompro.Services.Tasks;

/// <summary>
///     Represents a scheduled task for moderation activities, which runs at a regular interval.
///     This task is responsible for assigning possible moderators periodically.
/// </summary>
public class AddPossibleModeratorsTask : ScheduledTaskBase<IModerationService>
{
    // Specifies the interval at which the task should run. Set to 1 hour.
    private static readonly TimeSpan _interval = TimeSpan.FromHours(1);

    /// <summary>
    ///     Initializes a new instance of the <see cref="AddPossibleModeratorsTask" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the required services.</param>
    public AddPossibleModeratorsTask(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        // Set the interval for the scheduled task
        Interval = _interval;
    }

    /// <summary>
    ///     Executes the task to assign moderators asynchronously.
    ///     This method is called by the scheduler based on the specified interval.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the task.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
    protected override async Task ExecuteScopedAsync(CancellationToken cancellationToken)
    {
        // Call the service to assign moderators
        await ScopedService.AssignPossibleModeratorsAsync();
    }
}