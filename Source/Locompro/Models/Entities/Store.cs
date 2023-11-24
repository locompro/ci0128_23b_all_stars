using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Models.Entities;

using NetTopologySuite.Geometries;

/// <summary>
///     A store in a canton.
/// </summary>
public class Store
{
    [Key]
    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string Name { get; set; }

    [Required] public virtual Canton Canton { get; set; }

    [Required]
    [StringLength(35, MinimumLength = 1)]
    public string Address { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 4)]
    public string Telephone { get; set; }

    [Required] public Status Status { get; set; } = Status.Active;
    
    /*public virtual Point Location { get; set; }*/
}