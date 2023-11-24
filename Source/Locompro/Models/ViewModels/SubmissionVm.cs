using System.ComponentModel.DataAnnotations;
using Locompro.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Models.ViewModels;

public class SubmissionVm
{
    public SubmissionVm()
    {
    }

    public SubmissionVm(Submission submission, Func<Submission, string> getFormattedDate)
    {
        EntryTime = getFormattedDate(submission) ?? "";
        Price = submission.Price;
        Description = submission.Description ?? "";
        Status = submission.Status;
        UserId = submission.UserId ?? "";
        Rating = submission.Rating;
        NonFormatedEntryTime = submission.EntryTime;
    }

    [BindProperty] [StringLength(120)] public string Description { get; init; }

    public SubmissionStatus Status { get; set; }
    
    public string UserId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Ingresar el precio del producto.")]
    [Range(100, 10000000, ErrorMessage = "El precio debe estar entre ₡100 y ₡10.000.000.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El precio debe contener solamente números enteros.")]
    public int Price { get; init; }

    public string FormattedPrice => Price.ToString("C0");

    public string EntryTime { get; }

    public DateTime NonFormatedEntryTime { get; set; }

    public float Rating { get; set; }
}