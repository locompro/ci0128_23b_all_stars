using System.Security.Claims;
using Locompro.Common;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
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
        _mockReportService = new Mock<IReportService>();
        _mockLogger = new Mock<ILogger<ModerationService>>();

        _submissionService = new Mock<ISubmissionService>();
        _mockSearchService = new Mock<ISearchService>();


        // Setup mock logger factory to return the mock logger
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_mockLogger.Object);

        // Create an instance of the service to test
        _moderationService = new ModerationService(mockLoggerFactory.Object, _mockUserService.Object,
            _mockUserManagerService.Object, _submissionService.Object, _mockReportService.Object,
            _mockSearchService.Object);

        _mockReportService = new Mock<IReportService>();
    }

    private Mock<IUserService> _mockUserService = null!;
    private Mock<IUserManagerService> _mockUserManagerService = null!;
    private Mock<ILogger<ModerationService>> _mockLogger = null!;
    private Mock<ISubmissionService> _submissionService = null!;
    private ModerationService _moderationService = null!;
    private Mock<IReportService> _mockReportService = null!;
    private Mock<ISearchService> _mockSearchService = null!;

    /// <summary>ss
    ///     Tests that the AssignPossibleModeratorsAsync method successfully assigns the 'PossibleModerator' role
    ///     to all qualified users.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179- Sprint 2</author>
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
    /// <author>Brandon Alonso Mora Umaña - C15179 - Sprint 2</author>
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
            .ReturnsAsync((User)null!);

        // Act
        await _moderationService.AssignPossibleModeratorsAsync();

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)!));
    }

    /// <summary>
    ///     Tests that the AssignPossibleModeratorsAsync method logs an error if the role cannot be assigned to the user.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179 - Sprint 2</author>
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
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)!));
    }

    /// <summary>
    ///     Tests that the AssignPossibleModeratorsAsync method logs an informational message if the user is already a
    ///     moderator.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña - C15179 - Sprint 2</author>
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
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)!));
    }

    /// <summary>
    ///     If a moderator sends the reject submission action, they are added to list of rejecters
    ///     <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task ReportActionToRejectSubmissionAddsRejecter()
    {
        ModeratorActionDto modAction = new()
        {
            ModeratorId = "0",
            SubmissionUserId = "1",
            SubmissionEntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
            Action = ModeratorActions.RejectSubmission
        };

        await _moderationService.ActOnReport(modAction);

        _submissionService.Verify(
            service => service.AddSubmissionRejecter(
                It.Is<SubmissionKey>(sk => sk.UserId == modAction.SubmissionUserId
                                           && sk.EntryTime == modAction.SubmissionEntryTime),
                modAction.ModeratorId), Times.Once);
    }

    /// <summary>
    ///     If a moderator sends the approve submission action, they are added to list of approvers
    ///     <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
    /// </summary>
    [Test]
    public async Task ReportActionToApproveSubmissionAddsApprover()
    {
        ModeratorActionDto modAction = new()
        {
            ModeratorId = "0",
            SubmissionUserId = "1",
            SubmissionEntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
            Action = ModeratorActions.ApproveSubmission
        };

        await _moderationService.ActOnReport(modAction);

        _submissionService.Verify(
            service => service.AddSubmissionApprover(
                It.Is<SubmissionKey>(sk => sk.UserId == modAction.SubmissionUserId
                                           && sk.EntryTime == modAction.SubmissionEntryTime),
                modAction.ModeratorId), Times.Once);
    }

    /// <summary>
    ///     If an invalid action is sent, ArgumentOutOfRangeException is thrown
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 2</author>
    /// </summary>
    [Test]
    public void ReportActionInvalidOrDefaultThrowException()
    {
        ModeratorActionDto modAction = new()
        {
            SubmissionUserId = "1",
            SubmissionEntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
            Action = ModeratorActions.Default
        };

        ModeratorActionDto modAction1 = new()
        {
            SubmissionUserId = "1",
            SubmissionEntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
            Action = (ModeratorActions)3
        };

        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _moderationService.ActOnReport(modAction));
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await _moderationService.ActOnReport(modAction1));
        });
    }

    /// <summary>
    /// Tests that the IsUserPossibleModerator method returns true when the user is in the 'PossibleModerator' role.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public async Task IsUserPossibleModerator_ReturnsTrueWhenUserIsInRole()
    {
        // Arrange
        string userId = "user1";
        var user = new User { Id = userId };

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.PossibleModerator))
            .ReturnsAsync(true);

        // Act
        var result = await _moderationService.IsUserPossibleModerator(userId);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that the IsUserPossibleModerator method returns false when the user is not in the 'PossibleModerator' role.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public async Task IsUserPossibleModerator_ReturnsFalseWhenUserIsNotInRole()
    {
        // Arrange
        string userId = "user1";
        var user = new User { Id = userId };

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.PossibleModerator))
            .ReturnsAsync(false);

        // Act
        var result = await _moderationService.IsUserPossibleModerator(userId);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that the IsUserPossibleModerator method throws an ArgumentException when the user ID does not correspond to any user.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public void IsUserPossibleModerator_ThrowsArgumentExceptionIfUserNotFound()
    {
        // Arrange
        string userId = "user1";

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync((User)null);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _moderationService.IsUserPossibleModerator(userId));
    }

    /// <summary>
    /// Tests that the DecideOnModeratorRole method successfully adds the 'Moderator' role when the user accepts it.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public async Task DecideOnModeratorRole_AddsModeratorRoleWhenAccepted()
    {
        // Arrange
        string userId = "user1";
        bool didAcceptRole = true;
        var user = new User { Id = userId };

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.PossibleModerator))
            .ReturnsAsync(true);

        _mockUserManagerService.Setup(x =>
                x.AddClaimAsync(user, It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.Moderator)))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManagerService.Setup(x => x.DeleteClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.PossibleModerator)))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _moderationService.DecideOnModeratorRole(userId, didAcceptRole);

        // Assert
        _mockUserManagerService.Verify(
            x => x.AddClaimAsync(user, It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.Moderator)),
            Times.Once);
        _mockUserManagerService.Verify(
            x => x.DeleteClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.PossibleModerator)), Times.Once);
    }

    /// <summary>
    /// Tests that the DecideOnModeratorRole method successfully adds the 'RejectedModeratorRole' when the user declines it.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public async Task DecideOnModeratorRole_AddsRejectedModeratorRoleWhenDeclined()
    {
        // Arrange
        string userId = "user1";
        bool didAcceptRole = false;
        var user = new User { Id = userId };

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.PossibleModerator))
            .ReturnsAsync(true);

        _mockUserManagerService.Setup(x => x.AddClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.RejectedModeratorRole)))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManagerService.Setup(x => x.DeleteClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.PossibleModerator)))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _moderationService.DecideOnModeratorRole(userId, didAcceptRole);

        // Assert
        _mockUserManagerService.Verify(
            x => x.AddClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.RejectedModeratorRole)),
            Times.Once);
        _mockUserManagerService.Verify(
            x => x.DeleteClaimAsync(user,
                It.Is<Claim>(c => c.Type == ClaimTypes.Role && c.Value == RoleNames.PossibleModerator)), Times.Once);
    }

    /// <summary>
    /// Tests that the DecideOnModeratorRole method throws an ArgumentException when the user is not found.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public void DecideOnModeratorRole_ThrowsArgumentExceptionIfUserNotFound()
    {
        // Arrange
        string userId = "user1";
        bool didAcceptRole = true;

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync((User)null);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _moderationService.DecideOnModeratorRole(userId, didAcceptRole));
    }

    /// <summary>
    /// Tests that the DecideOnModeratorRole method throws an ArgumentException when the user is not in the 'PossibleModerator' role.
    /// </summary>
    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public void DecideOnModeratorRole_ThrowsArgumentExceptionIfUserNotInPossibleModeratorRole()
    {
        // Arrange
        string userId = "user1";
        bool didAcceptRole = true;
        var user = new User { Id = userId };

        _mockUserManagerService.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManagerService.Setup(x => x.IsInRoleAsync(user, RoleNames.PossibleModerator))
            .ReturnsAsync(false);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _moderationService.DecideOnModeratorRole(userId, didAcceptRole));
    }
}