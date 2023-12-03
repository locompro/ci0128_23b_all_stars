using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Services.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class ReportServiceTest
{
    [SetUp]
    public void Setup()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _reportRepository = new Mock<IReportRepository>();
        _mockLogger = new Mock<ILogger<ReportService>>();
        // Setup mock logger factory to return the mock logger
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_mockLogger.Object);

        _unitOfWork.Setup(u => u.GetSpecialRepository<IReportRepository>()).Returns(_reportRepository.Object);

        _reportService = new ReportService(_unitOfWork.Object, mockLoggerFactory.Object);
    }

    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IReportRepository> _reportRepository;
    private LoggerFactory _loggerFactory;
    private Mock<ILogger<ReportService>> _mockLogger = null!;
    private ReportService _reportService;

    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public async Task UpdateUserReportAsync_CreatesReportFromDtoAndSavesToDatabase()
    {
        // Arrange
        var reportDto = new UserReportDto
        {
            SubmissionUserId = "User123",
            SubmissionEntryTime = DateTime.Now,
            UserId = "Reporter456",
            Description = "This is a test report description."
        };

        _reportRepository.Setup(repo => repo.AddAsync(It.IsAny<UserReport>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _reportService.UpdateUserReportAsync(reportDto);

        // Assert
        _reportRepository.Verify(repo => repo.UpdateAsync(reportDto.SubmissionUserId, reportDto.SubmissionEntryTime,
            reportDto.UserId,
            It.Is<UserReport>(r =>
                r.SubmissionUserId == reportDto.SubmissionUserId &&
                r.SubmissionEntryTime == reportDto.SubmissionEntryTime &&
                r.UserId == reportDto.UserId &&
                r.Description == reportDto.Description
            )), Times.Once);
    }

    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
    [Test]
    public void UpdateUserReportAsync_ThrowsException_WhenUpdateFails()
    {
        // Arrange
        var userReportDto = new UserReportDto();

        _reportRepository.Setup(repo =>
                repo.UpdateAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<UserReport>()))
            .ThrowsAsync(new Exception());

        // Act & Assert
        var actualException = Assert.ThrowsAsync<Exception>(async () =>
            await _reportService.UpdateUserReportAsync(userReportDto));
    }

    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
    [Test]
    public async Task GetByUserIdAsync_ReturnsReportsForSpecificUser()
    {
        // Arrange
        var userId = "TestUserId";
        var expectedReports = new List<Report> { new Report(), new Report() };

        _reportRepository.Setup(repo => repo.GetByUserIdAsync(userId))
            .ReturnsAsync(expectedReports);

        // Act
        var actualReports = await _reportService.GetByUserId(userId);

        // Assert
        Assert.That(actualReports, Is.EqualTo(expectedReports));
    }

    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
    [Test]
    public async Task AddManyAutomaticReportsAsync_AddsReportsToDatabase()
    {
        // Arrange
        var listOfAutomaticReports = new List<AutoReportDto>
        {
            new AutoReportDto(),
            new AutoReportDto()
        };

        _reportRepository.Setup(repo => repo.AddOrUpdateManyAutomaticReports(It.IsAny<IEnumerable<AutoReport>>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _reportService.AddManyAutomaticReports(listOfAutomaticReports);

        // Assert
        _reportRepository.Verify(repo => repo.AddOrUpdateManyAutomaticReports(
            It.Is<IEnumerable<AutoReport>>(reports => reports.Count() == listOfAutomaticReports.Count)), Times.Once);
    }

    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 3</author>
    [Test]
    public void AddManyAutomaticReportsAsync_ThrowsException_WhenRepositoryOperationFails()
    {
        // Arrange
        var listOfAutomaticReports = new List<AutoReportDto>
        {
            new AutoReportDto(),
            new AutoReportDto()
        };

        _reportRepository.Setup(repo => repo.AddOrUpdateManyAutomaticReports(It.IsAny<IEnumerable<AutoReport>>()))
            .ThrowsAsync(new Exception());

        // Act & Assert
        var actualException = Assert.ThrowsAsync<Exception>(async () =>
            await _reportService.AddManyAutomaticReports(listOfAutomaticReports));
    }

    /// <author>Brandon Mora Umaña C15179 - Sprint 3</author>
    [Test]
    public async Task AddManyAutomaticReports_AddsReportsToDatabase()
    {
        // Arrange
        var autoReportDtos = new List<AutoReportDto>
        {
            new AutoReportDto
            {
                SubmissionEntryTime = new DateTime(2021, 1, 1),
                SubmissionUserId = "User 1",
                Product = "Cobija",
                Store = "Walmart",
                AveragePrice = 400,
                MinimumPrice = 100,
                MaximumPrice = 1000,
                Description = " ",
                Confidence = 0.5,
                Price = 500
            },
            new AutoReportDto
            {
                SubmissionEntryTime = new DateTime(2021, 1, 1),
                SubmissionUserId = "User 2",
                Product = "Cobija",
                Store = "Walmart",
                AveragePrice = 400,
                MinimumPrice = 100,
                MaximumPrice = 1000,
                Description = " ",
                Confidence = 0.5,
                Price = 500
            }
        };

        var autoReports = autoReportDtos.Select(dto => new AutoReport
        {
            SubmissionEntryTime = dto.SubmissionEntryTime,
            SubmissionUserId = dto.SubmissionUserId,
            AveragePrice = dto.AveragePrice,
            MinimumPrice = dto.MinimumPrice,
            MaximumPrice = dto.MaximumPrice,
            Description = dto.Description,
            Confidence = dto.Confidence
        }).ToList();

        _reportRepository.Setup(repo => repo.AddOrUpdateManyAutomaticReports(It.IsAny<List<AutoReport>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _reportService.AddManyAutomaticReports(autoReportDtos);

        // Assert
        _reportRepository.Verify(repo => repo.AddOrUpdateManyAutomaticReports(It.Is<List<AutoReport>>(reports =>
            reports.Count == autoReportDtos.Count &&
            reports.All(r => autoReportDtos.Any(dto =>
                dto.SubmissionEntryTime == r.SubmissionEntryTime &&
                dto.SubmissionUserId == r.SubmissionUserId &&
                dto.AveragePrice == r.AveragePrice &&
                dto.MinimumPrice == r.MinimumPrice &&
                dto.MaximumPrice == r.MaximumPrice &&
                dto.Description == r.Description &&
                dto.Confidence == r.Confidence
            )))), Times.Once);

        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <author>Brandon Mora Umaña C15179 - Sprint 3</author>
    [Test]
    public void AddManyAutomaticReports_ThrowsException_WhenAddFails()
    {
        // Arrange
        var autoReportDtos = new List<AutoReportDto>
        {
            new AutoReportDto
            {
                SubmissionEntryTime = new DateTime(2021, 1, 1),
                SubmissionUserId = "User 1",
                Product = "Cobija",
                Store = "Walmart",
                AveragePrice = 400,
                MinimumPrice = 100,
                MaximumPrice = 1000,
                Description = " ",
                Confidence = 0.5,
                Price = 500
            },
            new AutoReportDto
            {
                SubmissionEntryTime = new DateTime(2021, 1, 1),
                SubmissionUserId = "User 2",
                Product = "Cobija",
                Store = "Walmart",
                AveragePrice = 400,
                MinimumPrice = 100,
                MaximumPrice = 1000,
                Description = " ",
                Confidence = 0.5,
                Price = 500
            }
        };
        var exception = new Exception("Test exception");

        _reportRepository.Setup(repo => repo.AddOrUpdateManyAutomaticReports(It.IsAny<List<AutoReport>>()))
            .ThrowsAsync(exception);

        // Act & Assert
        var result = Assert.ThrowsAsync<Exception>(() => _reportService.AddManyAutomaticReports(autoReportDtos));
        Assert.That(result?.Message, Is.EqualTo(exception.Message));

        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true)!));
    }
}