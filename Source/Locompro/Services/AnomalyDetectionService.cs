using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Services.Domain;
using MathNet.Numerics.Distributions;

namespace Locompro.Services;

public class AnomalyDetectionService : Service, IAnomalyDetectionService
{
    private readonly IReportService _reportService;
    private readonly ISubmissionService _submissionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnomalyDetectionService"/> class.
    /// </summary>
    /// <param name="loggerFactory">Factory for creating loggers.</param>
    /// <param name="reportService">Service for handling reports.</param>
    /// <param name="submissionService">Service for managing submissions.</param>
    public AnomalyDetectionService(ILoggerFactory loggerFactory, IReportService reportService,
        ISubmissionService submissionService) : base(
        loggerFactory)
    {
        _reportService = reportService;
        _submissionService = submissionService;
    }

    /// <inheritdoc/>
    public async Task FindPriceAnomaliesAsync()
    {
        // Get all submissions
        var submissions = await _submissionService.GetAll();
        // Group them using product with same name and store with same name
        var groupedSubmissions = GroupSubmissionsByStoreAndProduct(submissions);

        // For each group, grab the automatic reports on anomalous submissions
        var listOfAutomaticReportsInItems = groupedSubmissions.Select(MakeReportsOnAnomalousSubmissions).ToList();

        // Flatten the list of lists
        var listOfAutomaticReports = listOfAutomaticReportsInItems.SelectMany(list => list).ToList();

        // Add the reports to the database
        try
        {
            await _reportService.AddManyAutomaticReports(listOfAutomaticReports);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add automatic reports to the database");
            throw;
        }
    }

    /// <summary>
    /// Groups submissions by store name and product name.
    /// </summary>
    /// <param name="submissions">The enumerable collection of submissions to be grouped.</param>
    /// <returns>A list of grouped submissions, where each group represents a unique combination of store and product.</returns>
    private List<GroupedSubmissions> GroupSubmissionsByStoreAndProduct(IEnumerable<Submission> submissions) =>
        submissions.GroupBy(submission => new { submission.StoreName, submission.Product.Name })
            .Select(group => new GroupedSubmissions
            {
                StoreName = group.Key.StoreName,
                ProductName = group.Key.Name,
                Submissions = group.ToList()
            })
            .ToList();

    /// <summary>
    /// Creates reports for submissions identified as anomalies based on their pricing.
    /// </summary>
    /// <param name="groupedSubmissions">The group of submissions containing submissions of the same product from the same store.</param>
    /// <returns>A list of <see cref="AutoReportDto"/> objects representing the reports for anomalous submissions.</returns>
    private List<AutoReportDto> MakeReportsOnAnomalousSubmissions(GroupedSubmissions groupedSubmissions)
    {
        var mean = CalculateMean(groupedSubmissions.Submissions);
        var minMaxPrice = CalculateMinMaxPrice(groupedSubmissions.Submissions);
        var standardDeviation = CalculateStandardDeviation(groupedSubmissions.Submissions, mean);

        if (standardDeviation == 0) return new List<AutoReportDto>();

        var anomalousSubmissions = groupedSubmissions.Submissions
            .Where(submission => CalculateZScore(submission, mean, standardDeviation) >= 1)
            .ToList();

        var tasks = anomalousSubmissions
            .Select(submission => ReportAnAnomalousSubmission(submission, mean, standardDeviation,
                minMaxPrice.minPrice,
                minMaxPrice.maxPrice)).ToList();

        return Task.WhenAll(tasks).Result.ToList();
    }


    /// <summary>
    /// Reports an anomalous submission and calculates confidence based on statistical analysis.
    /// </summary>
    /// <param name="submission">The submission to report.</param>
    /// <param name="mean">The mean price of submissions.</param>
    /// <param name="standardDeviation">The standard deviation of submission prices.</param>
    /// <param name="minPrice">The minimum price among submissions.</param>
    /// <param name="maxPrice">The maximum price among submissions.</param>
    /// <returns>A task that represents the asynchronous operation and returns an AutoReportDto.</returns>
    private Task<AutoReportDto> ReportAnAnomalousSubmission(Submission submission, double mean,
        double standardDeviation,
        int minPrice, int maxPrice)
    {
        // Create an AutoReportDto instance to store the report data.
        var autoReportDto = new AutoReportDto()
        {
            SubmissionUserId = submission.UserId,
            SubmissionEntryTime = submission.EntryTime,
            UserId = "Anomaly_Service",
            Price = submission.Price,
            MinimumPrice = minPrice,
            MaximumPrice = maxPrice,
            AveragePrice = mean,
            Confidence = CalculateConfidence(CalculateZScore(submission, mean, standardDeviation)) * 100,
            Description = submission.Description,
            Product = submission.Product.Name,
            Store = submission.StoreName
        };

        // Return the AutoReportDto as a completed task.
        return Task.FromResult(autoReportDto);
    }

    /// <summary>
    /// Calculates confidence based on the given Z-score.
    /// </summary>
    /// <param name="zScore">The Z-score of the submission's price.</param>
    /// <returns>The confidence value as a percentage.</returns>
    private double CalculateConfidence(double zScore)
    {
        // Calculate the confidence using the Z-score and the normal cumulative distribution function.
        var confidence = Normal.CDF(0, 1, zScore);
        return confidence;
    }

    /// <summary>
    /// Calculates the Z-score of a submission's price.
    /// </summary>
    /// <param name="submission">The submission to calculate the Z-score for.</param>
    /// <param name="mean">The mean price of submissions.</param>
    /// <param name="standardDeviation">The standard deviation of submission prices.</param>
    /// <returns>The Z-score of the submission's price.</returns>
    private double CalculateZScore(Submission submission, double mean, double standardDeviation) =>
        Math.Abs((submission.Price - mean) / standardDeviation);

    /// <summary>
    /// Calculates the minimum and maximum prices among a list of submissions.
    /// </summary>
    /// <param name="submissions">The list of submissions.</param>
    /// <returns>A tuple containing the minimum and maximum prices.</returns>
    private (int minPrice, int maxPrice) CalculateMinMaxPrice(List<Submission> submissions)
    {
        if (submissions == null || !submissions.Any())
        {
            throw new ArgumentException("Submissions list is empty or null.");
        }

        // Use Aggregate to find the minimum and maximum prices in the list of submissions.
        var minMaxPrice = submissions
            .Aggregate(
                (Min: int.MaxValue, Max: int.MinValue),
                (acc, submission) => (
                    Min: Math.Min(acc.Min, submission.Price),
                    Max: Math.Max(acc.Max, submission.Price))
            );

        return (minMaxPrice.Min, minMaxPrice.Max);
    }

    /// <summary>
    /// Calculates the mean price among a list of submissions.
    /// </summary>
    /// <param name="submissions">The list of submissions.</param>
    /// <returns>The mean price.</returns>
    private double CalculateMean(List<Submission> submissions) =>
        submissions.Average(submission => submission.Price);

    /// <summary>
    /// Calculates the standard deviation of submission prices.
    /// </summary>
    /// <param name="submissions">The list of submissions.</param>
    /// <param name="mean">The mean price of submissions.</param>
    /// <returns>The standard deviation of submission prices.</returns>
    private double CalculateStandardDeviation(List<Submission> submissions, double mean)
    {
        // Calculate the sum of squared differences from the mean.
        var sumOfSquaredDifferences = submissions
            .Select(submission => Math.Pow(submission.Price - mean, 2))
            .Sum();

        // Calculate the variance and return the square root to get the standard deviation.
        var variance = sumOfSquaredDifferences / submissions.Count;
        return Math.Sqrt(variance);
    }

    /// <summary>
    /// Represents a group of submissions for a store and product combination.
    /// </summary>
    class GroupedSubmissions
    {
        public string StoreName { get; set; }
        public string ProductName { get; set; }
        public List<Submission> Submissions { get; set; }
    }
}