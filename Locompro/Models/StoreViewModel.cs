using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    public class StoreViewModel
    {
        // Store Entity properties
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Required]
        [StringLength(35)]
        public string Address { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4)]
        public string Telephone { get; set; }

        // Selected Province and Canton
        public string Province { get; set; }
        public string Canton { get; set; }
    }
}