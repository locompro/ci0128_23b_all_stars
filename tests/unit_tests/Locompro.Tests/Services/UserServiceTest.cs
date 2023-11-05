using Moq;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Models.Results;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;


namespace Locompro.Tests.Services
{
    /// <summary>
    /// Contains unit tests for UserService class.
    /// Author: Brandon Alonso Mora Umaña.
    /// </summary>
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ILoggerFactory> _mockLoggerFactory;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            // Create mock instances
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLoggerFactory = new Mock<ILoggerFactory>();

            // Setup mock behavior
            _mockUnitOfWork.Setup(uow => uow.GetSpecialRepository<IUserRepository>()).Returns(_mockUserRepository.Object);

            // Instantiate the service with mocked dependencies
            _userService = new UserService(_mockUnitOfWork.Object, _mockLoggerFactory.Object);
        }

        /// <summary>
        /// Test to ensure GetQualifiedUserIDs returns expected user IDs.
        /// Author: Brandon Alonso Mora Umaña.
        /// </summary>
        [Test]
        public void GetQualifiedUserIDs_ReturnsExpectedUserIDs()
        {
            // Arrange
            var expectedUsers = new List<GetQualifiedUserIDsResult>
            {
                new GetQualifiedUserIDsResult { Id = "user1" },
                new GetQualifiedUserIDsResult { Id = "user2" }
            };
            _mockUserRepository.Setup(repo => repo.GetQualifiedUserIDs()).Returns(expectedUsers);

            // Act
            var result = _userService.GetQualifiedUserIDs();

            // Assert
            Assert.That(result, Is.EqualTo(expectedUsers));
        }
    }
}
