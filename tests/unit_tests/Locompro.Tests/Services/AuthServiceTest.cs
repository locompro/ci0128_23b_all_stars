using System.Security.Claims;
using Locompro.Data;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

/// <summary>
///     testing class for AuthService
/// </summary>
public class AuthServiceTest
{
    private readonly Mock<IUserEmailStore<User>> _emailStoreMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly AuthService _service;
    private readonly Mock<ISignInManagerService> _signInManagerMock;
    private readonly Mock<IUserManagerService> _userManagerMock;
    private readonly Mock<IUserStore<User>> _userStoreMock;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthServiceTest" /> class.
    ///     This constructor initializes mock objects for dependencies required by the <see cref="AuthService" /> class,
    ///     and sets up the <see cref="AuthService" /> instance with these mock objects.
    /// </summary>
    public AuthServiceTest()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _userStoreMock = new Mock<IUserStore<User>>();
        _emailStoreMock = new Mock<IUserEmailStore<User>>();
        _userManagerMock = new Mock<IUserManagerService>();
        _loggerMock = new Mock<ILogger<AuthService>>();

        _signInManagerMock = new Mock<ISignInManagerService>();

        var loggerFactoryMock = new Mock<ILoggerFactory>();
        loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

        _service = new AuthService(loggerFactoryMock.Object,
            _signInManagerMock.Object,
            _userManagerMock.Object,
            _userStoreMock.Object,
            _emailStoreMock.Object
        );
    }

    /// <summary>
    ///     Setup method to reset the state of mock objects before each test is executed.
    ///     Marked with the [SetUp] attribute to ensure it's run before each test within the fixture.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _userStoreMock.Reset();
        _emailStoreMock.Reset();
        _userManagerMock.Reset();
        _signInManagerMock.Reset();
        _loggerMock.Reset();
    }

    /// <summary>
    ///     Test if the user registration succeeds.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña C15179</author>
    [Test]
    public async Task Register_UserRegistrationSucceeds_ReturnsIdentityResultSuccess()
    {
        // Arrange
        var inputData = new RegisterVm
            { UserName = "TestUser", Email = "test@example.com", Password = "TestPassword123!" };
        var identityResult = IdentityResult.Success;

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), inputData.Password))
            .ReturnsAsync(identityResult);

        // Act

        var result = await _service.Register(inputData);

        // Assert
        Assert.That(result.Succeeded, Is.True);

        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
        _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<User>(), false), Times.Once);
        _emailStoreMock.Verify(x =>
            x.SetEmailAsync(It.IsAny<User>(), inputData.Email, It.IsAny<CancellationToken>()), Times.Once);
        _userStoreMock.Verify(x =>
            x.SetUserNameAsync(It.IsAny<User>(), inputData.UserName, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    ///     Test if the user registration fails.
    /// </summary>
    /// <author>Brandon Alonso Mora Umaña C15179</author>
    [Test]
    public async Task Register_UserRegistrationFails_ReturnsIdentityResultFailure()
    {
        // Arrange
        var inputData = new RegisterVm
            { UserName = "TestUser", Email = "test@example.com", Password = "Test" };
        var identityResult = IdentityResult.Failed();

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), inputData.Password))
            .ReturnsAsync(identityResult);

        // Act

        var result = await _service.Register(inputData);

        // Assert
        Assert.That(result.Succeeded, Is.False);

        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
        _emailStoreMock.Verify(x =>
            x.SetEmailAsync(It.IsAny<User>(), inputData.Email, It.IsAny<CancellationToken>()), Times.Once);
        _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<User>(), false), Times.Never);
        _userStoreMock.Verify(x =>
            x.SetUserNameAsync(It.IsAny<User>(), inputData.UserName, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    ///     Test if the user logout succeeds.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public async Task Logout_UserLoggedOut()
    {
        // Arrange
        var claimsPrincipal = CreateClaimsPrincipal();
        var httpContext = CreateMockHttpContext(claimsPrincipal);
        _signInManagerMock.Setup(s => s.Context).Returns(httpContext);

        // Act
        await _service.Logout();

        // Assert
        _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);

        _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => string.Equals("User someUserId logged out.", v.ToString())),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     Test if the user is logged in.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public void IsLoggedIn_UserIsLoggedIn()
    {
        // Arrange
        var claimsPrincipal = CreateClaimsPrincipal();
        var httpContext = CreateMockHttpContext(claimsPrincipal);
        _signInManagerMock.Setup(s => s.Context).Returns(httpContext);
        _signInManagerMock.Setup(s => s.IsSignedIn(claimsPrincipal)).Returns(true);

        // Act
        var isLoggedIn = _service.IsLoggedIn();

        // Assert
        Assert.That(isLoggedIn, Is.True);
    }


    /// <summary>
    ///     Test if the user is not logged in.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public void IsLoggedIn_UserIsNotLoggedIn()
    {
        // Arrange
        var claimsPrincipal = CreateClaimsPrincipal();
        var httpContext = CreateMockHttpContext(claimsPrincipal);

        _signInManagerMock.Setup(s => s.Context).Returns(httpContext);
        _signInManagerMock.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

        // Act
        var isLoggedIn = _service.IsLoggedIn();

        // Assert
        Assert.That(isLoggedIn, Is.False);
    }


    /// <summary>
    ///     Test if the user login succeeds.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public async Task Login_SuccessfulLogin()
    {
        // Arrange
        var inputData = new LoginVm { UserName = "TestUser", Password = "TestPassword123!" };

        _signInManagerMock.Setup(x =>
                x.PasswordSignInAsync(inputData.UserName, inputData.Password, inputData.RememberMe, false))
            .ReturnsAsync(SignInResult.Success);

        // Act
        var result = await _service.Login(inputData);

        // Assert
        Assert.That(result.Succeeded, Is.True);

        _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => string.Equals("User logged in.", v.ToString())),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     Test if the user login fails.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public async Task Login_FailedLogin()
    {
        // Arrange
        var inputData = new LoginVm { UserName = "TestUser", Password = "WrongPassword" };

        _signInManagerMock.Setup(x =>
                x.PasswordSignInAsync(inputData.UserName, inputData.Password, inputData.RememberMe, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _service.Login(inputData);
        // Assert
        Assert.That(result.Succeeded, Is.False);


        _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => string.Equals("User logged in.", v.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Never);
    }

    /// <summary>
    ///     Test to ensure the GetUserId method returns the correct user ID.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public void GetUserId_ReturnsUserId()
    {
        // Arrange
        var userId = "someUserId";
        var claimsPrincipal =
            new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }));
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(claimsPrincipal);

        // Act
        var result = _service.GetUserId();

        // Assert
        Assert.That(result, Is.EqualTo(userId));
    }

    /// <summary>
    ///     Test to verify that the IsCurrentPasswordCorrect method returns true when the password is correct.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public async Task IsCurrentPasswordCorrect_ReturnsTrue_WhenPasswordIsCorrect()
    {
        // Arrange
        var password = "correctPassword";
        var user = new User();
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(new ClaimsPrincipal());
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, password)).ReturnsAsync(true);

        // Act
        var result = await _service.IsCurrentPasswordCorrect(password);

        // Assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    ///     Test to ensure the ChangePassword method returns the correct IdentityResult.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public async Task ChangePassword_ReturnsIdentityResult()
    {
        // Arrange
        var currentPassword = "currentPassword";
        var newPassword = "newPassword";
        var user = new User();
        var identityResult = IdentityResult.Success;
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(new ClaimsPrincipal());
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.ChangePasswordAsync(user, currentPassword, newPassword))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _service.ChangePassword(currentPassword, newPassword);

        // Assert
        Assert.That(result, Is.EqualTo(identityResult));
    }

    /// <summary>
    ///     Test to ensure that the RefreshUserLogin method refreshes the login as expected.
    /// </summary>
    /// <author>A. Badilla Olivas B80874</author>
    [Test]
    public async Task RefreshUserLogin_RefreshesLogin()
    {
        // Arrange
        var user = new User();
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(new ClaimsPrincipal());
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        // Act
        await _service.RefreshUserLogin();

        // Assert
        _signInManagerMock.Verify(sm => sm.RefreshSignInAsync(user), Times.Once);
    }

    /// <summary>
    ///     Creates a <see cref="ClaimsPrincipal" /> object with a single claim of type
    ///     <see cref="ClaimTypes.NameIdentifier" />
    ///     containing the specified user ID.
    /// </summary>
    /// <param name="userId">The user ID to be embedded as a claim. Defaults to "someUserId" if not provided.</param>
    /// <returns>A <see cref="ClaimsPrincipal" /> object containing a single claim with the specified user ID.</returns>
    private static ClaimsPrincipal CreateClaimsPrincipal(string userId = "someUserId")
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }));
    }

    /// <summary>
    ///     Creates a mock <see cref="HttpContext" /> object with the specified <see cref="ClaimsPrincipal" /> set as the User
    ///     property.
    /// </summary>
    /// <param name="claimsPrincipal">
    ///     The <see cref="ClaimsPrincipal" /> to be set as the User property of the mock
    ///     <see cref="HttpContext" />.
    /// </param>
    /// <returns>
    ///     A mock <see cref="HttpContext" /> object with the specified <see cref="ClaimsPrincipal" /> set as the User
    ///     property.
    /// </returns>
    private static HttpContext CreateMockHttpContext(ClaimsPrincipal claimsPrincipal)
    {
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(h => h.User).Returns(claimsPrincipal);
        return httpContextMock.Object;
    }
}