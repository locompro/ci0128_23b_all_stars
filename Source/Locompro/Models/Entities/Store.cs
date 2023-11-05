using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Models.Entities
{
    /// <summary>
    /// A store in a canton.
    /// </summary>
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

        [Required]
        public Status Status { get; set; } = Status.Active;

        [Precision(18, 2)]
        public decimal Latitude { get; set; }

        [Precision(18, 2)]
        public decimal Longitude { get; set; }
    }
}
