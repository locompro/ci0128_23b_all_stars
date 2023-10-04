using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models;

public class Product
{
    [Key] [Required] public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string Name { get; set; }

    [StringLength(60)] public string Model { get; set; }

    [StringLength(60)] public string Brand { get; set; }
    
    [Required]
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<Category> Categories { get; set; }

    // TODO: Build pictures method automatic from DB?
}