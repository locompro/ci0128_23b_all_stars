using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.Entities;

public class Category
{
    [Key]
    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string Name { get; set; }

    public virtual Category Parent { get; set; }

    public virtual ICollection<Category> Children { get; set; }
    
    public virtual ICollection<Product> Products { get; set; }
}