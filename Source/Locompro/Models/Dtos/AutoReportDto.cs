namespace Locompro.Models.Dtos
{
    /// <summary>
    /// Represents a data transfer object (DTO) for reporting an anomalous submission.
    /// </summary>
    public class AutoReportDto
    {
        /// <summary>
        /// Gets or sets the user ID of the submission.
        /// </summary>
        public string SubmissionUserId { get; set; }

        /// <summary>
        /// Gets or sets the entry time of the submission.
        /// </summary>
        public DateTime SubmissionEntryTime { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the reporting service.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product associated with the submission.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the name of the store associated with the submission.
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        /// Gets or sets the description of the submission.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the confidence level of the report as a percentage.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the price of the submission.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Gets or sets the minimum price among submissions.
        /// </summary>
        public int MinimumPrice { get; set; }

        /// <summary>
        /// Gets or sets the maximum price among submissions.
        /// </summary>
        public int MaximumPrice { get; set; }

        /// <summary>
        /// Gets or sets the average price among submissions.
        /// </summary>
        public double AveragePrice { get; set; }
    }
}