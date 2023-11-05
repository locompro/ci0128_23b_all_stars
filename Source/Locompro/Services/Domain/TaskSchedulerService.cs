using Locompro.Services.Tasks;

namespace Locompro.Services.Domain;

/// <summary>
/// Schedules and manages the execution of background tasks.
/// </summary>
public sealed class TaskSchedulerService : IHostedService, IDisposable
{
    private readonly IEnumerable<IScheduledTask> _tasks;
    private readonly ILogger<TaskSchedulerService> _logger;
    private readonly List<Timer> _timers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskSchedulerService"/> class.
    /// </summary>
    /// <param name="tasks">The collection of scheduled tasks to be managed.</param>
    /// <param name="logger">The logger to be used for logging information and errors.</param>
    public TaskSchedulerService(IEnumerable<IScheduledTask> tasks, ILogger<TaskSchedulerService> logger)
    {
        _tasks = tasks;
        _logger = logger;
    }

    /// <summary>
    /// Starts the task scheduler service, scheduling the configured tasks to run at their defined intervals.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the start process.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting scheduler");
        foreach (var task in _tasks)
        {
            var timer = new Timer(
                callback: ExecuteTask,
                state: new TaskState { Task = task, Token = cancellationToken },
                dueTime: TimeSpan.Zero,
                period: task.Interval);

            _timers.Add(timer);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the task scheduler service, ensuring that all scheduled tasks are no longer invoked.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the stop process.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping scheduler");
        foreach (var timer in _timers)
        {
            timer.Change(Timeout.Infinite, 0);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes the given scheduled task.
    /// </summary>
    /// <param name="state">The state object containing the scheduled task and cancellation token.</param>
    private async void ExecuteTask(object state)
    {
        var taskState = (TaskState)state;
        _logger.LogInformation("Executing task {TaskName}.", taskState.Task.GetType().Name);
        try
        {
            await taskState.Task.ExecuteAsync(taskState.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred executing task {TaskName}.", taskState.Task.GetType().Name);
        }

        _logger.LogInformation("Finished executing task {TaskName}.", taskState.Task.GetType().Name);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="TaskSchedulerService"/>.
    /// </summary>
    public void Dispose()
    {
        foreach (var timer in _timers)
        {
            timer?.Dispose();
        }
    }

    /// <summary>
    /// Represents the state of a scheduled task, including the task itself and a cancellation token.
    /// </summary>
    private class TaskState
    {
        /// <summary>
        /// Gets the scheduled task to be executed.
        /// </summary>
        public IScheduledTask Task { get; init; }

        /// <summary>
        /// Gets the cancellation token associated with the scheduled task.
        /// </summary>
        public CancellationToken Token { get; init; }
    }
}