using Locompro.Services.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Locompro.Tests.Services.Tasks;

[TestFixture]
public class ScheduledTaskBaseTest
{
    private Mock<IServiceProvider> _mockServiceProvider;
    private Mock<IServiceScope> _mockServiceScope;
    private Mock<IServiceScopeFactory> _mockServiceScopeFactory;
    private Mock<IModerationService> _mockScopedService;

    /// <summary>
    /// Sets up the mocks for the service provider, service scope, service scope factory, and the
    /// scoped service required for testing the ScheduledTaskBase class.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [SetUp]
    public void SetUp()
    {
        // Mock the scoped service that our scheduled task requires
        _mockScopedService = new Mock<IModerationService>();

        // Mock the IServiceScope which will return our mock scoped service
        _mockServiceScope = new Mock<IServiceScope>();
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

        _mockServiceScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);

        // Mock the IServiceScopeFactory to return our mock scope
        _mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(_mockServiceScope.Object);

        // Mock the IServiceProvider to return our mock IServiceScopeFactory
        _mockServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(_mockServiceScopeFactory.Object);

        // Mock the IServiceProvider to return our mock scoped service
        // This setup is correct and should allow the IServiceProvider within the scope to return the mock
        _mockServiceProvider.Setup(x => x.GetService(typeof(IModerationService)))
            .Returns(_mockScopedService.Object);
    }

    /// <summary>
    /// Tests that the ExecuteAsync method on an instance of a ScheduledTaskBase-derived class
    /// correctly invokes the ExecuteScopedAsync method of the scoped service.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [Test]
    public async Task ExecuteAsync_CallsExecuteScopedAsync()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var scheduledTask = new ConcreteScheduledTask(_mockServiceProvider.Object);

        // Act
        await scheduledTask.ExecuteAsync(cancellationToken);

        // Assert
        _mockScopedService.Verify(x => x.AssignPossibleModeratorsAsync(), Times.Once);
    }

    /// <summary>
    /// Provides a concrete implementation of ScheduledTaskBase for the purpose of testing the abstract class.
    /// </summary>
    public class ConcreteScheduledTask : ScheduledTaskBase<IModerationService>
    {
        public ConcreteScheduledTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task ExecuteScopedAsync(CancellationToken cancellationToken)
        {
            await ScopedService.AssignPossibleModeratorsAsync();
        }
    }
}