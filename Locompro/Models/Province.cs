using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    public class Province
    {
        [Key]
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Key]
        [Required]
        public Country Country { get; set; }
    }
}