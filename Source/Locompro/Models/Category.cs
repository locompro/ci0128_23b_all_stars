using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Locompro.Models;

public class Category
{
    [Key]
    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string Name { get; set; }
    [JsonIgnore]
    public virtual Category Parent { get; set; }
    [JsonIgnore]
    public virtual ICollection<Category> Children { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; }
}