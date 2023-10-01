using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Locompro.Models
{
    public class Country
    {
        [Key]
        [Required]
        [StringLength(60)]
        public string Name { get; set; }
    }
}