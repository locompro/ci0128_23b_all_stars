using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.ViewModels;

/// <summary>
/// Incoming new product details.
/// </summary>
public class ProductViewModel
{
    [Required(ErrorMessage = "Ingresar el nombre del producto.")]
    [StringLength(60, MinimumLength = 1)]
    [DisplayName("Nombre")]
    public string Name { get; set; }
    
    [StringLength(60)]
    [DisplayName("Modelo")]
    public string Model { get; set; }

    [StringLength(60)]
    [DisplayName("Marca")]
    public string Brand { get; set; }
    
    [StringLength(60)]
    [DisplayName("Categoría")]
    public string Category { get; set; }
}