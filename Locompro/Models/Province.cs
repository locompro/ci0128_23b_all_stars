using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locompro.Models;

public class Province
{
    [Key] [Required] [Column(Order = 0)] public virtual Country Country { get; set; }

    [Key] [Required] [Column(Order = 1)] public string Name { get; set; }

    public virtual ICollection<Canton> Cantons { get; set; }
}