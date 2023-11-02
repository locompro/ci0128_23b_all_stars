using System.Security.Claims;
using Locompro.Areas.Identity.ViewModels;
using Locompro.Data;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Locompro.Tests.Services
{
    public class AuthServiceTest
    {
        private Mock<IUnitOfWork>? _unitOfWorkMock;
        private Mock<IUserStore<User>>? _userStoreMock;
        private Mock<IUserEmailStore<User>>? _emailStoreMock;
        private Mock<UserManager<User>>? _userManagerMock;
        private Mock<ILogger<AuthService>>? _loggerMock;
        private Mock<SignInManager<User>>? _signInManagerMock;
        private AuthService? _service;


        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userStoreMock = new Mock<IUserStore<User>>();
            _emailStoreMock = new Mock<IUserEmailStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                _userStoreMock.Object, null, null, null, null, null, null, null, null
            );
            _loggerMock = new Mock<ILogger<AuthService>>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "someUserId"),
                // Add other claims as needed
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(hc => hc.User).Returns(claimsPrincipal);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            contextAccessorMock.Setup(ca => ca.HttpContext).Returns(httpContextMock.Object);

            var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
            var optionsAccessorMock = new Mock<IOptions<IdentityOptions>>();
            var loggerSignInManagerMock = new Mock<ILogger<SignInManager<User>>>();
            var schemesMock = new Mock<IAuthenticationSchemeProvider>();
            var confirmationMock = new Mock<IUserConfirmation<User>>();

            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                userClaimsPrincipalFactoryMock.Object,
                optionsAccessorMock.Object,
                loggerSignInManagerMock.Object,
                schemesMock.Object,
                confirmationMock.Object
            );

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

            _service = new AuthService(
                _unitOfWorkMock.Object,
                loggerFactoryMock.Object,
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _userStoreMock.Object,
                _emailStoreMock.Object
            );
        }

        public Mock<SignInManager<User>> SignInManagerMock => _signInManagerMock;
        public Mock<UserManager<User>> UserManagerMock => _userManagerMock;


        /// <summary>
        /// Test if the user registration succeeds.
        /// </summary>
        /// <author>Brandon Alonso Mora Umaña C15179</author>
        [Test]
        public async Task Register_UserRegistrationSucceeds_ReturnsIdentityResultSuccess()
        {
            // Arrange
            var inputData = new RegisterViewModel
                { UserName = "TestUser", Email = "test@example.com", Password = "TestPassword123!" };
            var identityResult = IdentityResult.Success;

            if (_userManagerMock != null)
            {
                _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), inputData.Password))
                    .ReturnsAsync(identityResult);

                // Act
                if (_service != null)
                {
                    var result = await _service.Register(inputData);

                    // Assert
                    Assert.That(result.Succeeded, Is.True);
                }

                _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
            }

            _signInManagerMock?.Verify(x => x.SignInAsync(It.IsAny<User>(), false, null), Times.Once);
            _emailStoreMock?.Verify(x =>
                x.SetEmailAsync(It.IsAny<User>(), inputData.Email, It.IsAny<CancellationToken>()), Times.Once);
            _userStoreMock?.Verify(x =>
                x.SetUserNameAsync(It.IsAny<User>(), inputData.UserName, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Test if the user registration fails.
        /// </summary>
        /// <author>Brandon Alonso Mora Umaña C15179</author>
        [Test]
        public async Task Register_UserRegistrationFails_ReturnsIdentityResultFailure()
        {
            // Arrange
            var inputData = new RegisterViewModel
                { UserName = "TestUser", Email = "test@example.com", Password = "Test" };
            var identityResult = IdentityResult.Failed();

            _userManagerMock?.Setup(x => x.CreateAsync(It.IsAny<User>(), inputData.Password))
                .ReturnsAsync(identityResult);

            // Act
            if (_service != null)
            {
                var result = await _service.Register(inputData);

                // Assert
                Assert.That(result.Succeeded, Is.False);
            }

            _userManagerMock?.Verify(x => x.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
            _emailStoreMock?.Verify(x =>
                x.SetEmailAsync(It.IsAny<User>(), inputData.Email, It.IsAny<CancellationToken>()), Times.Once);
            _signInManagerMock?.Verify(x => x.SignInAsync(It.IsAny<User>(), false, null), Times.Never);
            _userStoreMock?.Verify(x =>
                x.SetUserNameAsync(It.IsAny<User>(), inputData.UserName, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Test if the user logout succeeds.
        /// </summary>
        /// <author>A. Badilla Olivas B80874</author>
        [Test]
        public async Task Logout_UserLoggedOut()
        {
            if (_service != null) await _service.Logout();

            _signInManagerMock?.Verify(x => x.SignOutAsync(), Times.Once);

            _loggerMock?.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => string.Equals($"User someUserId logged out.", v.ToString())),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }


        /// <summary>
        /// Test if the user is logged in.
        /// </summary>
        /// <author>A. Badilla Olivas B80874</author>
        [Test]
        public void IsLoggedIn_UserIsLoggedIn()
        {
            _signInManagerMock?.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var isLoggedIn = _service != null && _service.IsLoggedIn();

            Assert.That(isLoggedIn, Is.True);
        }

        /// <summary>
        /// Test if the user is not logged in.
        /// </summary>
        /// <author>A. Badilla Olivas B80874</author>
        [Test]
        public void IsLoggedIn_UserIsNotLoggedIn()
        {
            _signInManagerMock?.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

            var isLoggedIn = _service != null && _service.IsLoggedIn();

            Assert.That(isLoggedIn, Is.False);
        }

        /// <summary>
        /// Test if the user login succeeds.
        /// </summary>
        /// <author>A. Badilla Olivas B80874</author>
        [Test]
        public async Task Login_SuccessfulLogin()
        {
            var inputData = new LoginViewModel { UserName = "TestUser", Password = "TestPassword123!" };

            _signInManagerMock?.Setup(x =>
                    x.PasswordSignInAsync(inputData.UserName, inputData.Password, inputData.RememberMe, false))
                .ReturnsAsync(SignInResult.Success);

            if (_service != null)
            {
                var result = await _service.Login(inputData);

                Assert.That(result.Succeeded, Is.True);
            }

            _loggerMock?.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => string.Equals("User logged in.", v.ToString())),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        /// <summary>
        /// Test if the user login fails.
        /// </summary>
        /// <author>A. Badilla Olivas B80874</author>
        [Test]
        public async Task Login_FailedLogin()
        {
            var inputData = new LoginViewModel { UserName = "TestUser", Password = "WrongPassword" };

            _signInManagerMock?.Setup(x =>
                    x.PasswordSignInAsync(inputData.UserName, inputData.Password, inputData.RememberMe, false))
                .ReturnsAsync(SignInResult.Failed);

            if (_service != null)
            {
                var result = await _service.Login(inputData);

                Assert.That(result.Succeeded, Is.False);
            }

            _loggerMock?.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => string.Equals("User logged in.", v.ToString())),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Never);
        }
        [Test]
    public void GetUserId_ReturnsUserId()
    {
        var userId = "someUserId";
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId) }));
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(claimsPrincipal);

        var result = _service.GetUserId();

        Assert.AreEqual(userId, result);
    }

    [Test]
    public async Task IsCurrentPasswordCorrect_ReturnsTrue_WhenPasswordIsCorrect()
    {
        var password = "correctPassword";
        var user = new User();
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(new ClaimsPrincipal());
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, password)).ReturnsAsync(true);

        var result = await _service.IsCurrentPasswordCorrect(password);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task ChangePassword_ReturnsIdentityResult()
    {
        var currentPassword = "currentPassword";
        var newPassword = "newPassword";
        var user = new User();
        var identityResult = IdentityResult.Success;
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(new ClaimsPrincipal());
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.ChangePasswordAsync(user, currentPassword, newPassword)).ReturnsAsync(identityResult);

        var result = await _service.ChangePassword(currentPassword, newPassword);

        Assert.AreEqual(identityResult, result);
    }

    [Test]
    public async Task RefreshUserLogin_RefreshesLogin()
    {
        var user = new User();
        _signInManagerMock.Setup(sm => sm.Context.User).Returns(new ClaimsPrincipal());
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        await _service.RefreshUserLogin();

        _signInManagerMock.Verify(sm => sm.RefreshSignInAsync(user), Times.Once);
    }
    }
}