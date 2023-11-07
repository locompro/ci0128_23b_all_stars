namespace Locompro.Services.Tasks;

/// <summary>
///     Base class for scheduled tasks that require scoped services.
/// </summary>
/// <typeparam name="T">The type of the scoped service required by the task.</typeparam>
public abstract class ScheduledTaskBase<T> : IScheduledTask where T : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ScheduledTaskBase{T}" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for creating scopes.</param>
    protected ScheduledTaskBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    ///     Gets the service provider used to create scopes and retrieve services.
    /// </summary>
    private IServiceProvider ServiceProvider { get; }

    /// <summary>
    ///     Gets or sets the scoped service to be used during task execution.
    /// </summary>
    protected T ScopedService { get; set; }

    /// <summary>
    ///     Gets the interval at which the task should be run.
    /// </summary>
    public TimeSpan Interval { get; protected init; }

    /// <summary>
    ///     Executes the task asynchronously. This method creates a scope for the task and retrieves the scoped service.
    /// </summary>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = ServiceProvider.CreateScope();
        ScopedService = scope.ServiceProvider.GetRequiredService<T>();
        await ExecuteScopedAsync(cancellationToken);
    }

    /// <summary>
    ///     When implemented in a derived class, executes the scoped task asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation of the scoped task.</returns>
    protected abstract Task ExecuteScopedAsync(CancellationToken cancellationToken);
}