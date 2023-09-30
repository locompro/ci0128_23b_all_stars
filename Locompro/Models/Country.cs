using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    public class Country
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Province> Provinces { get; set; }
    }
}