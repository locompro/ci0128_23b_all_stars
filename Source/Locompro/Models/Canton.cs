﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Locompro.Models
{
    /// <summary>
    /// A canton for a province.
    /// </summary>
    public class Canton
    {
        [Required]
        public string CountryName { get; set; }
        
        [Required]
        public string ProvinceName { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }
        
        public virtual Province Province { get; set; }
    }
}