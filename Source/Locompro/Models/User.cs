using System.Collections.Generic;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models;

/// <summary>
/// User in the Locompro system. This class extends the <see cref="IdentityUser"/> class provided by ASP.NET Core Identity,
/// adding additional properties such as Name, Address, Rating, Status, and a collection of Submissions associated with the user.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    /// <value>
    /// The full name of the user.
    /// </value>
    [StringLength(40, MinimumLength = 1)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    /// <value>
    /// The address of the user.
    /// </value>
    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the rating of the user.
    /// </summary>
    /// <value>
    /// The rating of the user. Default value is 0.
    /// </value>
    public float Rating { get; set; } = 0;

    /// <summary>
    /// Gets or sets the status of the user.
    /// </summary>
    /// <value>
    /// The status of the user. Default value is <see cref="Status.Active"/>.
    /// </value>
    [Required]
    public Status Status { get; set; } = Status.Active;

    /// <summary>
    /// Gets or sets the collection of submissions associated with the user.
    /// </summary>
    /// <value>
    /// The collection of submissions associated with the user.
    /// </value>
    public virtual ICollection<Submission> Submissions { get; set; }
}
