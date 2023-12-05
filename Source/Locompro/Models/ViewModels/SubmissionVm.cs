using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        Description = submission.Description ?? "N/A";
        Status = submission.Status;
        UserId = submission.UserId ?? "";
        Username = submission.User.UserName ?? "N/A";
        Rating = submission.Rating;
        NonFormatedEntryTime =
            submission.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
    }

    [BindProperty] [StringLength(120)] public string Description { get; init; }

    public SubmissionStatus Status { get; set; }
    
    public string UserId { get; set; }
    public string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Ingresar el precio del producto.")]
    [Range(100, 10000000, ErrorMessage = "El precio debe estar entre ₡100 y ₡10.000.000.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El precio debe contener solamente números enteros.")]
    public int Price { get; init; }

    public string FormattedPrice => Price.ToString("C0").TrimStart('$');

    public string EntryTime { get; }

    public string NonFormatedEntryTime { get; set; }

    public float Rating { get; set; }
}