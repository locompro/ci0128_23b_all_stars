using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.Entities
{
    /// <summary>
    /// A country with provinces.
    /// </summary>
    public class Country
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Province> Provinces { get; set; }
    }
}