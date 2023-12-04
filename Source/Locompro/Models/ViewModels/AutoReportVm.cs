﻿using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

public class AutoReportVm
{
    [Required] public string SubmissionUserId { get; set; }

    [Required] public DateTime SubmissionEntryTime { get; set; }

    [Required] public string UserId { get; set; }
    public string Product { get; set; }
    public string Store { get; set; }
    public string Description { get; set; }

    [DisplayFormat(DataFormatString = "{0:F4}%")]
    public double Confidence { get; set; }

    public int Price { get; set; }
    public int MinimumPrice { get; set; }
    public int MaximumPrice { get; set; }

    [DisplayFormat(DataFormatString = "{0:F0}")]
    public double AveragePrice { get; set; }
}