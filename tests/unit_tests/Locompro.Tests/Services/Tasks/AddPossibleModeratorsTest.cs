using Locompro.Services.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Locompro.Tests.Services.Tasks;

/// <summary>
///     Contains unit tests for the AddPossibleModeratorsTask class, ensuring that the scheduled task
///     correctly calls the moderation service to assign possible moderators.
/// </summary>
/// <author>Brandon Alonso Mora Umaña - C15179</author>
[TestFixture]
public class AddPossibleModeratorsTaskTest
{
    /// <summary>
    ///     Set up the test environment, mocking the necessary services and scope factory.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [SetUp]
    public void SetUp()
    {
        // Mock the moderation service that our scheduled task requires
        _mockModerationService = new Mock<IModerationService>();

        // Mock the service scope to return the mock moderation service
        _mockServiceScope = new Mock<IServiceScope>();
        _mockServiceScope.Setup(x => x.ServiceProvider.GetService(typeof(IModerationService)))
            .Returns(_mockModerationService.Object);

        // Mock the service scope factory to return our mock service scope
        _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceScopeFactory.Setup(x => x.CreateScope())
            .Returns(_mockServiceScope.Object);

        // Mock the service provider to return the service scope factory
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(_mockServiceScopeFactory.Object);
    }

    private Mock<IServiceProvider> _mockServiceProvider;
    private Mock<IServiceScope> _mockServiceScope;
    private Mock<IServiceScopeFactory> _mockServiceScopeFactory;
    private Mock<IModerationService> _mockModerationService;

    /// <summary>
    ///     Tests that the ExecuteAsync method on AddPossibleModeratorsTask calls the AssignPossibleModeratorsAsync
    ///     method on the IModerationService exactly once, verifying that the task correctly triggers
    ///     the moderation action.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [Test]
    public async Task ExecuteAsync_CallsAssignPossibleModeratorsAsync()
    {
        // Arrange
        var cancellationToken = new CancellationToken(false);
        var task = new AddPossibleModeratorsTask(_mockServiceProvider.Object);

        // Act
        await task.ExecuteAsync(cancellationToken);

        // Assert
        _mockModerationService.Verify(service => service.AssignPossibleModeratorsAsync(), Times.Once);
    }
}