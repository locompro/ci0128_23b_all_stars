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
    private Mock<ICrudRepository<Report, string>> _reportRepository;
    private Mock<ILoggerFactory> _loggerFactory;
    private ReportService _reportService;

    [SetUp]
    public void Setup()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _reportRepository = new Mock<ICrudRepository<Report, string>>();
        _loggerFactory = new Mock<ILoggerFactory>();

        _unitOfWork.Setup(u => u.GetCrudRepository<Report, string>()).Returns(_reportRepository.Object);

        _reportService = new ReportService(_unitOfWork.Object, _loggerFactory.Object);
    }

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task Add_CreatesReportFromDtoAndSavesToDatabase()
    {
        // Arrange
        var reportDto = new ReportDto
        {
            SubmissionUserId = "User123",
            SubmissionEntryTime = DateTime.Now,
            UserId = "Reporter456",
            Description = "This is a test report description."
        };

        _reportRepository.Setup(repo => repo.AddAsync(It.IsAny<Report>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _reportService.Add(reportDto);

        // Assert
        _reportRepository.Verify(repo => repo.AddAsync(It.Is<Report>(r =>
            r.SubmissionUserId == reportDto.SubmissionUserId &&
            r.SubmissionEntryTime == reportDto.SubmissionEntryTime &&
            r.UserId == reportDto.UserId &&
            r.Description == reportDto.Description
        )), Times.Once);
    }
}
