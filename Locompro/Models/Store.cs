using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    public class Store
    {
        [Key]
        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public virtual Canton Canton { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string Address { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4)]
        public string Telephone { get; set; }

        [Precision(18, 2)]
        public decimal Latitude { get; set; }

        [Precision(18, 2)]
        public decimal Longitude { get; set; }

        public Status Status { get; set; } = Status.Active;
    }
}
