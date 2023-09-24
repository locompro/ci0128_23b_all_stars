using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Locompro.Models
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
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

        public float Rating { get; set; } = 0;

        public UserStatus Status { get; set; } = UserStatus.Default;

        public virtual ICollection<UserRole> Roles { get; set; }
        public User()
        {
            Roles = new HashSet<UserRole>();
        }
    }

    public enum UserStatus
    {
        Default,
        Active,
        Deleted
    }

    public enum Role
    {
        Default,
        Moderator
    }

    public class UserRole
    {
        [Key, Column(Order = 0)]
        [ForeignKey("User")]
        public string Username { get; set; }
        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public Role Role { get; set; }
    }
}

/*
 * USER(Username, password, email, name, address, rating, status, rols)
 */