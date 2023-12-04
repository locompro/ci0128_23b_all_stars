using System.Security.Authentication;
using Locompro.Pages.Moderation;
using Locompro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Tests.Pages.Moderation
{
    /// <summary>
    /// This class contains unit tests for the ModeratorPageModel class.
    /// </summary>
    [TestFixture]
    public class ModeratorPageModelTest
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IModerationService> _moderationServiceMock;
        private Mock<ISearchService> _searchServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private ModeratorPageModel _moderatorPageModel;

        [SetUp]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthService>();
            _configurationMock = new Mock<IConfiguration>();
            _moderationServiceMock = new Mock<IModerationService>();
            _searchServiceMock = new Mock<ISearchService>();
            _userServiceMock = new Mock<IUserService>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _moderatorPageModel = new ModeratorPageModel(
                _loggerFactoryMock.Object,
                _httpContextAccessorMock.Object,
                _searchServiceMock.Object,
                _moderationServiceMock.Object,
                _authServiceMock.Object,
                _userServiceMock.Object,
                _configurationMock.Object);
        }

        /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
        [Test]
        public Task OnGet_UserNotLoggedIn_ThrowsAuthenticationException()
        {
            // Arrange
            _authServiceMock.Setup(x => x.IsLoggedIn()).Returns(false);

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationException>(() => _moderatorPageModel.OnGet(null, null));
            return Task.CompletedTask;
        }
    }
}
