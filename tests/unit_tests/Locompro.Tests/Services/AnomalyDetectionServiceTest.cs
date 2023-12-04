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
        _mockLogger = new Mock<ILogger<AnomalyDetectionService>>();
        // Setup mock logger factory to return the mock logger

        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_mockLogger.Object);
        _anomalyDetectionService = new AnomalyDetectionService(
            _loggerFactoryMock.Object,
            _reportServiceMock.Object,
            _submissionServiceMock.Object);
    }

    private Mock<IReportService> _reportServiceMock;
    private Mock<ISubmissionService> _submissionServiceMock;
    private Mock<ILoggerFactory> _loggerFactoryMock;
    private Mock<ILogger<AnomalyDetectionService>> _mockLogger;
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

    /// <author> Brandon Mora Umana - C15179 </author>
    /// Sprint 3
    [Test]
    public async Task FindPriceAnomaliesAsync_CallsLoggerOnException()
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
            .ThrowsAsync(new Exception());

        // Act
        try
        {
            await _anomalyDetectionService.FindPriceAnomaliesAsync();
        }
        catch (Exception)
        {
            // ignored
        }

        // Assert
        // Verify that the logger was called once
        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }

    /// <author> Brandon Mora Umana - C15179 </author>
    [Test]
    public async Task FIndPriceAnomalies_CalculatesZIndex()
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
            Price = 500,
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

        await _anomalyDetectionService.FindPriceAnomaliesAsync();

        // Assert
        // Verify that the submission service's GetAll method was called once
        _submissionServiceMock.Verify(service => service.GetAll(), Times.Once);
    }

    /// <author> Brandon Mora Umana - C15179 </author>
    [Test]
    public async Task FindPriceAnomalies_EmptySubmissionList()
    {
        var submissions = new List<Submission>();
        if (submissions == null) throw new ArgumentNullException(nameof(submissions));

        // Mock the submission service to return the test submissions

        _submissionServiceMock.Setup(service => service.GetAll()).ReturnsAsync(submissions);

        await _anomalyDetectionService.FindPriceAnomaliesAsync();

        // Assert
        // Verify that the submission service's GetAll method was called once
        _submissionServiceMock.Verify(service => service.GetAll(), Times.Once);
    }
}