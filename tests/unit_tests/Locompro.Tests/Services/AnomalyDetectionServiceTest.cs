using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class AnomalyDetectionServiceTests
{
    [SetUp]
    public void Setup()
    {
        _reportServiceMock = new Mock<IReportService>();
        _submissionServiceMock = new Mock<ISubmissionService>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _anomalyDetectionService = new AnomalyDetectionService(
            _loggerFactoryMock.Object,
            _reportServiceMock.Object,
            _submissionServiceMock.Object);
    }

    private Mock<IReportService> _reportServiceMock;
    private Mock<ISubmissionService> _submissionServiceMock;
    private Mock<ILoggerFactory> _loggerFactoryMock;
    private AnomalyDetectionService _anomalyDetectionService;

    /// <summary>
    /// Tests that the FindPriceAnomaliesAsync method calls the report service's AddManyAutomaticReports method once.
    /// </summary>
    /// <author> Brandon Mora Umana - C15179</author>
    /// Sprint 3
    [Test]
    public async Task FindPriceAnomaliesAsync_CallsSubmissionService()
    {
        // Arrange
        var submission1 = new Submission
        {
            EntryTime = new DateTime(2021, 1, 1),
            Price = 100,
            Product = new Product
            {
                Name = "Product 1"
            },
            Store = new Store
            {
                Name = "Store 1"
            },
            UserId = "User 1"
        };
        var submission2 = new Submission
        {
            EntryTime = new DateTime(2021, 1, 1),
            Price = 100,
            Product = new Product
            {
                Name = "Product 1"
            },
            Store = new Store
            {
                Name = "Store 1"
            },
            UserId = "User 2"
        };
        var submissions = new List<Submission> { submission1, submission2 };

        // Mock the submission service to return the test submissions
        _submissionServiceMock.Setup(service => service.GetAll()).ReturnsAsync(submissions);

        _reportServiceMock
            .Setup(service => service.AddManyAutomaticReports(It.IsAny<List<AutoReportDto>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _anomalyDetectionService.FindPriceAnomaliesAsync();

        // Assert
        // Verify that the submission service's GetAll method was called once
        _submissionServiceMock.Verify(service => service.GetAll(), Times.Once);
    }

    /// <summary>
    /// Tests that the FindPriceAnomaliesAsync method calls the report service's AddManyAutomaticReports method once.
    /// </summary>
    /// <author> Brandon Mora Umana - C15179 </author>
    /// Sprint 3
    [Test]
    public async Task FindPriceAnomaliesAsync_CallsReportService()
    {
        // Arrange
        var submission1 = new Submission
        {
            EntryTime = new DateTime(2021, 1, 1),
            Price = 100,
            Product = new Product
            {
                Name = "Product 1"
            },
            Store = new Store
            {
                Name = "Store 1"
            },
            UserId = "User 1"
        };
        var submission2 = new Submission
        {
            EntryTime = new DateTime(2021, 1, 1),
            Price = 100,
            Product = new Product
            {
                Name = "Product 1"
            },
            Store = new Store
            {
                Name = "Store 1"
            },
            UserId = "User 2"
        };
        var submissions = new List<Submission> { submission1, submission2 };

        var groupedSubmissions = new List<AnomalyDetectionService.GroupedSubmissions>
        {
            new()
            {
                StoreName = "Store 1",
                ProductName = "Product 1",
                Submissions = new List<Submission> { submission1, submission2 }
            }
        };

        // Mock the submission service to return the test submissions
        _submissionServiceMock.Setup(service => service.GetAll()).ReturnsAsync(submissions);

        _reportServiceMock
            .Setup(service => service.AddManyAutomaticReports(It.IsAny<List<AutoReportDto>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _anomalyDetectionService.FindPriceAnomaliesAsync();

        // Assert
        // Verify that the report service's AddManyAutomaticReports method was called once
        _reportServiceMock.Verify(service => service.AddManyAutomaticReports(It.IsAny<List<AutoReportDto>>()),
            Times.Once);
    }
}