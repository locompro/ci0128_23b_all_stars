using System.Collections.Generic;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models;

public class User : IdentityUser
{
    [StringLength(40, MinimumLength = 1)]
    public string Name { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; }

    public float Rating { get; set; } = 0;

    [Required]
    public Status Status { get; set; } = Status.Active;
    
    public virtual ICollection<Submission> Submissions { get; set; }
}
