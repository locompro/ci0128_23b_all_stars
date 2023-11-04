using Locompro.Services.Tasks;

namespace Locompro.Services.Domain;

/// <summary>
/// Schedules a task to run in the background. 
/// </summary>
public sealed class TaskSchedulerService : IHostedService, IDisposable
{
    private readonly IScheduledTask _task;
    private readonly ILogger<TaskSchedulerService> _logger;
    private Timer _timer;

    public TaskSchedulerService(IScheduledTask task, ILogger<TaskSchedulerService> logger)
    {
        _task = task;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting scheduler for task {_task.GetType().Name}.");
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, _task.Interval);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Stopping scheduler for task {_task.GetType().Name}.");
        _timer.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void ExecuteTask(object state)
    {
        // TODO: In case the task is complex, a revelant cancellation token should be passed.
        _logger.LogInformation("Executing task {TaskName}.", _task.GetType().Name);
        _ = _task.ExecuteAsync(CancellationToken.None);

        _logger.LogInformation("Executed task {TaskName}.", _task.GetType().Name);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}