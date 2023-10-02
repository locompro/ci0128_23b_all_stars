using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Locompro.Models
{
    /// <summary>
    /// A country with provinces.
    /// </summary>
    public class Country
    {
        [Key]
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        public virtual ICollection<Province> Provinces { get; set; }
    }
}