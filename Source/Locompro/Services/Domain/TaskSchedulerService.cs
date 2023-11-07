using Locompro.Services.Tasks;

namespace Locompro.Services.Domain;

/// <summary>
///     Schedules and manages the execution of background tasks.
/// </summary>
public sealed class TaskSchedulerService : IHostedService
{
    private readonly ILogger<TaskSchedulerService> _logger;
    private readonly IEnumerable<IScheduledTask> _tasks;
    private readonly List<Timer> _timers = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskSchedulerService" /> class.
    /// </summary>
    /// <param name="tasks">The collection of scheduled tasks to be managed.</param>
    /// <param name="logger">The logger to be used for logging information and errors.</param>
    public TaskSchedulerService(IEnumerable<IScheduledTask> tasks, ILogger<TaskSchedulerService> logger)
    {
        _tasks = tasks;
        _logger = logger;
    }

    /// <summary>
    ///     Starts the task scheduler service, scheduling the configured tasks to run at their defined intervals.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the start process.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting scheduler");
        foreach (var task in _tasks)
        {
            var timer = new Timer(
                ExecuteTask,
                new TaskState { Task = task, Token = cancellationToken },
                TimeSpan.Zero,
                task.Interval);

            _timers.Add(timer);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Stops the task scheduler service, ensuring that all scheduled tasks are no longer invoked.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the stop process.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping scheduler");
        foreach (var timer in _timers.ToList())
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("StopAsync operation was canceled.");
                return Task.FromCanceled(cancellationToken);
            }

            timer.Change(Timeout.Infinite, 0);
            timer.Dispose();
            _timers.Remove(timer);
        }

        _logger.LogInformation("All timers have been stopped and disposed.");
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Executes the given scheduled task.
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
    ///     Represents the state of a scheduled task, including the task itself and a cancellation token.
    /// </summary>
    private class TaskState
    {
        /// <summary>
        ///     Gets the scheduled task to be executed.
        /// </summary>
        public IScheduledTask Task { get; init; }

        /// <summary>
        ///     Gets the cancellation token associated with the scheduled task.
        /// </summary>
        public CancellationToken Token { get; init; }
    }
}