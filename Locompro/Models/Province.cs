using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    /// <summary>
    /// A province with cantons in a country.
    /// </summary>
    public class Province
    {
        [Required]
        public string CountryName { get; set; }
        
        [Required]
        [StringLength(60)]
        public string Name { get; set; }
        
        public virtual Country Country { get; set; }

        public virtual ICollection<Canton> Cantons { get; set; }
    }
}