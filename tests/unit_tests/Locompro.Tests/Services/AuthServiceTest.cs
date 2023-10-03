﻿using System.Security.Claims;
using Locompro.Areas.Identity.ViewModels;
using Locompro.Models;
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
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<IUserEmailStore<User>> _emailStoreMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<ILogger<RegisterViewModel>> _loggerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private AuthService _service;

        [SetUp]
        public void SetUp()
        {
            _userStoreMock = new Mock<IUserStore<User>>();
            _emailStoreMock = new Mock<IUserEmailStore<User>>();
            _userManagerMock =
                new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _loggerMock = new Mock<ILogger<RegisterViewModel>>();

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContextMock = new Mock<HttpContext>();
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

            _service = new AuthService(_signInManagerMock.Object, _userManagerMock.Object, _userStoreMock.Object,
                _loggerMock.Object, _emailStoreMock.Object);
        }


        [Test]
        public async Task Register_UserRegistrationSucceeds_ReturnsIdentityResultSuccess()
        {
            // Arrange
            var inputData = new RegisterViewModel
                { UserName = "TestUser", Email = "test@example.com", Password = "TestPassword123!" };
            var identityResult = IdentityResult.Success;

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), inputData.Password))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _service.Register(inputData);

            // Assert
            Assert.That(result.Succeeded, Is.True);
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), inputData.Password), Times.Once);
            _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<User>(), false, null), Times.Once);
            _emailStoreMock.Verify(x =>
                x.SetEmailAsync(It.IsAny<User>(), inputData.Email, It.IsAny<CancellationToken>()), Times.Once);
            _userStoreMock.Verify(x =>
                x.SetUserNameAsync(It.IsAny<User>(), inputData.UserName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Register_UserRegistrationFails_ReturnsIdentityResultFailure()
        {
            // Arrange
            var inputData = new RegisterViewModel
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
            _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<User>(), false, null), Times.Never);
            _userStoreMock.Verify(x =>
                x.SetUserNameAsync(It.IsAny<User>(), inputData.UserName, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Logout_UserLoggedOut()
        {
            await _service.Logout();

            _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
            _loggerMock.Verify(l => l.Log(
                    LogLevel.Information, 
                    It.IsAny<EventId>(), 
                    It.Is<It.IsAnyType>((v, t) => string.Equals("User logged out.", v.ToString())), 
                    null, 
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), 
                Times.Once);        }

        [Test]
        public void IsLoggedIn_UserIsLoggedIn()
        {
            _signInManagerMock.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var isLoggedIn = _service.IsLoggedIn();

            Assert.That(isLoggedIn, Is.True);
        }

        [Test]
        public void IsLoggedIn_UserIsNotLoggedIn()
        {
            _signInManagerMock.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

            var isLoggedIn = _service.IsLoggedIn();

            Assert.That(isLoggedIn, Is.False);
        }
    }
    
}