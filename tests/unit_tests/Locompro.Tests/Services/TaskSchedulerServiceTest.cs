using Locompro.Services.Domain;
using Locompro.Services.Tasks;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

/// <summary>
///     A mock scheduled task for unit testing the TaskSchedulerService.
///     Author: Brandon Alonso Mora Umaña.
/// </summary>
public class MockScheduledTask : IScheduledTask
{
    /// <summary>
    ///     Gets or sets a value indicating whether the task has been executed.
    /// </summary>
    public bool Executed { get; set; }

    /// <summary>
    ///     Gets the interval at which the task should run.
    /// </summary>
    public TimeSpan Interval { get; } = TimeSpan.FromMilliseconds(600); // Short interval for testing

    /// <summary>
    ///     Simulates the task's asynchronous execution.
    ///     Author: Brandon Alonso Mora Umaña - Sprint 2.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token that the task will observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Simulate some work
        await Task.Delay(50, cancellationToken);
        Executed = true;
    }
}

/// <summary>
///     Test fixture for the TaskSchedulerService.
///     Author: Brandon Alonso Mora Umaña - Sprint 2.
/// </summary>
[TestFixture]
public class TaskSchedulerServiceTests
{
    /// <summary>
    ///     Sets up the test environment before each test.
    ///     Author: Brandon Alonso Mora Umaña. - Sprint 2
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<TaskSchedulerService>>();
        _mockScheduledTask = new MockScheduledTask();
        _service = new TaskSchedulerService(new[] { _mockScheduledTask }, _mockLogger.Object);
    }

    private Mock<ILogger<TaskSchedulerService>> _mockLogger;
    private TaskSchedulerService _service;
    private MockScheduledTask _mockScheduledTask;

    /// <summary>
    ///     Ensures that tasks are scheduled correctly when StartAsync is called.
    ///     Author: Brandon Alonso Mora Umaña - Sprint 2
    /// </summary>
    [Test]
    public async Task StartAsync_SchedulesTasksCorrectly()
    {
        // Act
        await _service.StartAsync(CancellationToken.None);

        // Assert
        // We wait for a bit more than the interval to see if the task has been executed
        await Task.Delay(_mockScheduledTask.Interval + TimeSpan.FromMilliseconds(50));
        Assert.IsTrue(_mockScheduledTask.Executed);
    }

    /// <summary>
    ///     Ensures that tasks are stopped correctly when StopAsync is called.
    ///     Author: Brandon Alonso Mora Umaña - Sprint 2
    /// </summary>
    [Test]
    public async Task StopAsync_StopsTasks()
    {
        // Arrange
        await _service.StartAsync(CancellationToken.None);

        // Act
        await _service.StopAsync(CancellationToken.None);

        await Task.Delay(_mockScheduledTask.Interval + TimeSpan.FromMilliseconds(600));

        // Assert
        // Reset execution flag
        _mockScheduledTask.Executed = false;
        // We wait for a bit more than the interval to see if the task does not execute
        await Task.Delay(_mockScheduledTask.Interval + TimeSpan.FromMilliseconds(600));
        Assert.IsFalse(_mockScheduledTask.Executed);
    }
}