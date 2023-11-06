using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Models.Entities;

/// <summary>
///     User of the application.
/// </summary>
public class User : IdentityUser
{
    [StringLength(40, MinimumLength = 1)] public string Name { get; set; }

    [StringLength(100, MinimumLength = 1)] public string Address { get; set; }

    public float Rating { get; set; } = 0;

    [Required] public Status Status { get; set; } = Status.Active;

    public virtual ICollection<Submission> Submissions { get; set; }
}