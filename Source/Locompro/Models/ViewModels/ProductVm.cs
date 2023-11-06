using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Locompro.Common;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Incoming new product details.
/// </summary>
public class ProductVm
{
    [Required(ErrorMessage = "Seleccionar un producto.")]
    public int Id { get; set; }

    [RequiredIf("IsExistingProduct", false, "Ingresar el nombre del producto.")]
    [StringLength(60, MinimumLength = 1)]
    [DisplayName("Nombre")]
    public string PName { get; set; } // not a typo

    [StringLength(60)]
    [DisplayName("Modelo")]
    public string Model { get; set; }

    [StringLength(60)]
    [DisplayName("Marca")]
    public string Brand { get; set; }

    [StringLength(60)]
    [DisplayName("Categoría")]
    public string Category { get; set; }

    public bool IsExistingProduct()
    {
        return -1 != Id;
    }
}