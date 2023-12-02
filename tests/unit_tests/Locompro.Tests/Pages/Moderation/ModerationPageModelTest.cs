using Locompro.Pages.Moderation;
using Locompro.Services;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Pages.Moderation;

[TestFixture]
public class ModeratorPageModelTests
{
    [SetUp]
    public void Setup()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockModerationService = new Mock<IModerationService>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockHttpContextAccessor = new Mock<HttpContextAccessor>();
        _mockUserService = new Mock<IUserService>();
        _moderationServiceMock = new Mock<IModerationService>();
        _mockSearchService = new Mock<ISearchService>();
        _moderatorPageModel = new ModeratorPageModel(
            _mockLoggerFactory.Object,
            _mockHttpContextAccessor.Object,
            _mockSearchService.Object,
            _mockModerationService.Object,
            _mockAuthService.Object,
            _mockUserService.Object,
            _mockConfiguration.Object);
    }

    private ModeratorPageModel _moderatorPageModel;
    private Mock<HttpContextAccessor> _mockHttpContextAccessor;
    private Mock<ILoggerFactory> _mockLoggerFactory;
    private Mock<IAuthService> _mockAuthService;
    private Mock<IModerationService> _mockModerationService;
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<ISearchService> _mockSearchService;
    private Mock<IUserService> _mockUserService;
    private Mock<IModerationService> _moderationServiceMock;

    /// <author>Brandon Mora Umaña C15179 - Sprint 3</author>
    [Test]
    public void OnGetAsync_WhenCalled_ReturnsPage()
    {
        // Arrange
        _mockAuthService.Setup(x => x.IsLoggedIn()).Returns(true);
        // Act
        var result = _moderatorPageModel.OnGet(0, 0);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}