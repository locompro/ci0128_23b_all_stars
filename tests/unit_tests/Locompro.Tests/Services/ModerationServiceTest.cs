using System.Security.Claims;
using Locompro.Common;
using Locompro.Models.Entities;
using Locompro.Models.Results;
using Locompro.Services;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class ModerationServiceTests
{
    [SetUp]
    public void SetUp()
    {
        // Create mock instances
        _mockUserService = new Mock<IUserService>();
        _mockUserManagerService = new Mock<IUserManagerService>();
        _mockLogger = new Mock<ILogger<ModerationService>>();

        // Setup mock logger factory to return the mock logger
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_mockLogger.Object);

        // Create an instance of the service to test
        _moderationService = new ModerationService(mockLoggerFactory.Object, _mockUserService.Object,
            _mockUserManagerService.Object);
    }

    private Mock<IUserService> _mockUserService;
    private Mock<IUserManagerService> _mockUserManagerService;
    private Mock<ILogger<ModerationService>> _mockLogger;
    private ModerationService _moderationService;

    /// <summary>
    ///     Tests that the AssignPossibleModeratorsAsync method successfully assigns the 'PossibleModerator' role
    ///     to all qualified users.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [Test]
    public async Task AssignPossibleModeratorsAsync_AssignsRolesToQualifiedUsers()
    {
        // Arrange
        var qualifiedUsers = new List<GetQualifiedUserIDsResult>
        {
            new() { Id = "user1" },
            new() { Id = "user2" }
        };

        _mockUserService.Setup(x => x.GetQualifiedUserIDs())
            .Returns(qualifiedUsers);

        _mockUserManagerService.Setup(x =>
            x.GetClaimsOfTypesAsync( // This is the method that checks if the user has the role
                It.IsAny<User>(),
                It.IsAny<string>())).ReturnsAsync(new List<Claim>());

        var user = new User(); // Assuming User is the type used by IUserManagerService

        _mockUserManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.Moderator))
            .ReturnsAsync(false);

        _mockUserManagerService.Setup(x => x.AddClaimAsync(user, It.IsAny<Claim>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _moderationService.AssignPossibleModeratorsAsync();

        // Assert
        _mockUserManagerService.Verify(
            x => x.AddClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.PossibleModerator)),
            Times.Exactly(2));
    }

    /// <summary>
    ///     Tests that the AssignPossibleModeratorsAsync method logs an error when a user cannot be found.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [Test]
    public async Task AssignPossibleModeratorsAsync_LogsErrorIfUserNotFound()
    {
        // Arrange
        var qualifiedUsers = new List<GetQualifiedUserIDsResult>
        {
            new() { Id = "user1" }
        };

        _mockUserService.Setup(x => x.GetQualifiedUserIDs())
            .Returns(qualifiedUsers);

        _mockUserManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        // Act
        await _moderationService.AssignPossibleModeratorsAsync();

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)));
    }

    /// <summary>
    ///     Tests that the AssignPossibleModeratorsAsync method logs an error if the role cannot be assigned to the user.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [Test]
    public async Task AssignPossibleModeratorsAsync_LogsErrorIfRoleCannotBeAssigned()
    {
        // Arrange
        var qualifiedUsers = new List<GetQualifiedUserIDsResult>
        {
            new() { Id = "user1" }
        };

        _mockUserService.Setup(x => x.GetQualifiedUserIDs())
            .Returns(qualifiedUsers);

        _mockUserManagerService.Setup(x =>
            x.GetClaimsOfTypesAsync( // This is the method that checks if the user has the role
                It.IsAny<User>(),
                It.IsAny<string>())).ReturnsAsync(new List<Claim>());

        var user = new User(); // Assuming User is the type used by IUserManagerService

        _mockUserManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.Moderator))
            .ReturnsAsync(false);

        _mockUserManagerService.Setup(x => x.AddClaimAsync(user, It.IsAny<Claim>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        await _moderationService.AssignPossibleModeratorsAsync();

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)));
    }

    /// <summary>
    ///     Tests that the AssignPossibleModeratorsAsync method logs an informational message if the user is already a
    ///     moderator.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179</author>
    [Test]
    public async Task AssignPossibleModeratorsAsync_LogsInformationIfUserIsAlreadyModerator()
    {
        // Arrange
        var qualifiedUsers = new List<GetQualifiedUserIDsResult>
        {
            new() { Id = "user1" }
        };

        _mockUserService.Setup(x => x.GetQualifiedUserIDs())
            .Returns(qualifiedUsers);

        var user = new User(); // Assuming User is the type used by IUserManagerService

        _mockUserManagerService.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.Moderator))
            .ReturnsAsync(true);

        _mockUserManagerService.Setup(x =>
                x.GetClaimsOfTypesAsync( // This is the method that checks if the user has the role
                    It.IsAny<User>(),
                    It.IsAny<string>()))
            .ReturnsAsync(new List<Claim> { new(ClaimTypes.Role, RoleNames.Moderator) });

        // Act
        await _moderationService.AssignPossibleModeratorsAsync();

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)));
    }
}