using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locompro.Models
{
    public class Canton
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public Province Province { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        [StringLength(60)]
        public string Name { get; set; }
    }
}