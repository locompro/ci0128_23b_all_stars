using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Locompro.Models {
    public class User {
        [Key]
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Address { get; set; }
        // default 0
        public float Rating { get; set; } = 0;
        public int Status { get; set; } = 0;
        public int Roles { get; set; } = 0;

    }
}

/*
 * USER(Username, password, email, name, address, rating, status, rols)
 */