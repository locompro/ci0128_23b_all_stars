using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    /// <summary>
    /// A province with cantons in a country.
    /// </summary>
    public class Province
    {
        [Key]
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Key]
        [Required]
        public virtual Country Country { get; set; }

        public virtual ICollection<Canton> Cantons { get; set; }
    }
}