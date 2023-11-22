using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Models.ViewModels;

/// <summary>
///     Incoming new store details.
/// </summary>
public class StoreVm
{
    // Store Entity properties
    [Required(ErrorMessage = "Seleccionar una tienda.")]
    [StringLength(60)]
    [DisplayName("Nombre")]
    public string SName { get; set; } // not a typo

    [Required(ErrorMessage = "Ingresar la dirección de la tienda.")]
    [StringLength(35)]
    [DisplayName("Dirección")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Ingresar el teléfono de la tienda.")]
    [StringLength(15, MinimumLength = 4, ErrorMessage = "El teléfono deber tener al menos 4 dígitos.")]
    [RegularExpression(@"^[\d\s\-\+\(\)]+$", ErrorMessage = "El teléfono contiene caracteres no permitidos.")]
    [DisplayName("Teléfono")]
    public string Telephone { get; set; }

    // Selected Province and SubmissionByCanton
    [Required(ErrorMessage = "Seleccionar la provincia de la tienda.")]
    [DisplayName("Provincia")]
    public string Province { get; set; }

    [Required(ErrorMessage = "Seleccionar el cantón de la tienda.")]
    [DisplayName("Cantón")]
    public string Canton { get; set; }

    [Required(ErrorMessage = "Debe seleccionar la ubicación de la tienda.")]
    
    [Precision(18, 2)]
    public decimal Latitude { get; set; } = 0;

    [Precision(18, 2)]
    public decimal Longitude { get; set; } = 0;
    
    public string MapGeneratedAddress { get; set; } = string.Empty;

    public bool IsExistingStore()
    {
        return !string.IsNullOrEmpty(SName) &&
               string.IsNullOrEmpty(Address) &&
               string.IsNullOrEmpty(Telephone) &&
               string.IsNullOrEmpty(Province) &&
               string.IsNullOrEmpty(Canton);
    }
}