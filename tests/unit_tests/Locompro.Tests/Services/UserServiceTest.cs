using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Results;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

/// <summary>
///     Contains unit tests for UserService class.
///     Author: Brandon Alonso Mora Umaña.
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
    ///     Test to ensure GetQualifiedUserIDs returns expected user IDs.
    ///     Author: Brandon Alonso Mora Umaña - Sprint 2
    /// </summary>
    [Test]
    public void GetQualifiedUserIDs_ReturnsExpectedUserIDs()
    {
        // Arrange
        var expectedUsers = new List<GetQualifiedUserIDsResult>
        {
            new() { Id = "user1" },
            new() { Id = "user2" }
        };
        _mockUserRepository.Setup(repo => repo.GetQualifiedUserIDs()).Returns(expectedUsers);

        // Act
        var result = _userService.GetQualifiedUserIDs();

        // Assert
        Assert.That(result, Is.EqualTo(expectedUsers));
    }

    /// <summary>
    ///     Test method to verify if the correct count of submissions by a user is returned.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetSubmissionsCountByUser_ReturnsExpectedCount()
    {
        // Arrange
        var expectedCount = 5; // replace with the expected count
        _mockUserRepository.Setup(repo => repo.GetSubmissionsCountByUser(It.IsAny<string>())).Returns(expectedCount);

        // Act
        var result = _userService.GetSubmissionsCountByUser("testUser");

        // Assert
        Assert.That(result, Is.EqualTo(expectedCount));
    }

    /// <summary>
    ///     Test method to verify if the correct count of reported submissions by a user is returned.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetReportedSubmissionsCountByUser_ReturnsExpectedCount()
    {
        // Arrange
        var expectedCount = 5; // replace with the expected count
        _mockUserRepository.Setup(repo => repo.GetReportedSubmissionsCountByUser(It.IsAny<string>()))
            .Returns(expectedCount);

        // Act
        var result = _userService.GetReportedSubmissionsCountByUser("testUser");

        // Assert
        Assert.That(result, Is.EqualTo(expectedCount));
    }

    /// <summary>
    ///     Test method to verify if the correct count of rated submissions by a user is returned.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetRatedSubmissionsCountByUser()
    {
        // Arrange
        var expectedCount = 5; // replace with the expected count
        _mockUserRepository.Setup(repo => repo.GetRatedSubmissionsCountByUser(It.IsAny<string>()))
            .Returns(expectedCount);

        // Act
        var result = _userService.GetRatedSubmissionsCountByUser("testUser");

        // Assert
        Assert.That(result, Is.EqualTo(expectedCount));
    }

    /// <summary>
    ///     Generates a list of test user information for testing purposes.
    /// </summary>
    /// <returns>A list of MostReportedUsersResult.</returns>
    public List<MostReportedUsersResult> GetTestList()
    {
        List<MostReportedUsersResult> testList = new();
        for (var i = 0; i < 12; i++)
            testList.Add(GenerateRandomTestUserInfo());
        return testList;
    }

    /// <summary>
    ///     Generates a random user information instance for testing.
    /// </summary>
    /// <returns>A MostReportedUsersResult instance with randomly generated data.</returns>
    public MostReportedUsersResult GenerateRandomTestUserInfo()
    {
        var testNumber = new Random().Next(0, 10000000);

        var userInfo = new MostReportedUsersResult
        {
            UserRating = new Random().NextSingle() * 5,
            UserName = $"test user{testNumber}",
            ReportedSubmissionCount = testNumber,
            TotalUserSubmissions = testNumber + 10
        };
        return userInfo;
    }

    /// <summary>
    ///     Test method to verify if the method returns the expected size of the list of most reported users.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetMostReportedUser_ReturnsExpectedListSize()
    {
        // Arrange
        var testListOfMostReportedUser = GetTestList();
        _mockUserRepository.Setup(repo => repo.GetMostReportedUsersInfo()).Returns(testListOfMostReportedUser);
        // Act
        var results = _userService.GetMostReportedUsersInfo();
        // Assert 
        Assert.That(results, Has.Count.LessThan(testListOfMostReportedUser.Count));
        Assert.That(results, Has.Count.EqualTo(10));
    }

    /// <summary>
    ///     Test method to verify if the list of most reported users is returned in the correct order.
    ///     sprint 3
    /// </summary>
    /// <author> A. Badilla Olivas B80874 </author>
    [Test]
    public void GetMostReportedUser_ReturnsOrderList()
    {
        // Arrange
        var testListOfMostReportedUser = GetTestList();
        _mockUserRepository.Setup(repo => repo.GetMostReportedUsersInfo()).Returns(testListOfMostReportedUser);
        // Act
        var results = _userService.GetMostReportedUsersInfo();
        // Assert 
        Assert.That(results.Count, Is.LessThan(testListOfMostReportedUser.Count));
        for (var i = 0; i < results.Count - 1; i++)
            Assert.That(results[i].ReportedSubmissionCount, Is.GreaterThan(results[i + 1].ReportedSubmissionCount));
    }
}