using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Models.ViewModels;

public class SubmissionViewModel
{
    [BindProperty]
    [StringLength(120)]
    public string Description { get; set; }
    
    public string UserId { get; set; }
    
    [BindProperty]
    [Required(ErrorMessage = "Ingresar el precio del producto.")]
    [Range(100, 10000000, ErrorMessage = "El precio debe estar entre ₡100 y ₡10.000.000.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El precio debe contener solamente números enteros.")]
    public int Price { get; set; }
}