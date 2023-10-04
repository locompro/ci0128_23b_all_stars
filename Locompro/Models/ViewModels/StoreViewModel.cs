using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class StoreViewModel
    {
        // Store Entity properties
        [Required(ErrorMessage = "Ingresar el nombre de la tienda.")]
        [StringLength(60)]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ingresar la dirección de la tienda.")]
        [StringLength(35)]
        [DisplayName("Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ingresar el teléfono de la tienda.")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "El teléfono deber tener al menos 4 dígitos.")]
        [DisplayName("Teléfono")]
        public string Telephone { get; set; }

        // Selected Province and Canton
        [Required(ErrorMessage = "Seleccionar la provincia de la tienda.")]
        [DisplayName("Provincia")]
        public string Province { get; set; }
        
        [Required(ErrorMessage = "Seleccionar el cantón de la tienda.")]
        [DisplayName("Cantón")]
        public string Canton { get; set; }
    }
}