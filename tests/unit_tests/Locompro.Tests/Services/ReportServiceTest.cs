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
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IReportRepository> _reportRepository;
    private ILoggerFactory _loggerFactory;
    private ReportService _reportService;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _reportRepository = new Mock<IReportRepository>();
        _loggerFactory = new LoggerFactory();

        _unitOfWork.Setup(u => u.GetSpecialRepository<IReportRepository>()).Returns(_reportRepository.Object);

        _reportService = new ReportService(_unitOfWork.Object, _loggerFactory);
    }

    /// <author>Ariel Arevalo Alvarado B50562 - Sprint 2</author>
    [Test]
    public async Task UpdateAsync_CreatesReportFromDtoAndSavesToDatabase()
    {
        // Arrange
        var reportDto = new ReportDto
        {
            SubmissionUserId = "User123",
            SubmissionEntryTime = DateTime.Now,
            UserId = "Reporter456",
            Description = "This is a test report description."
        };

        _reportRepository.Setup(repo => repo.AddAsync(It.IsAny<UserReport>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _reportService.UpdateAsync(reportDto);

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
}