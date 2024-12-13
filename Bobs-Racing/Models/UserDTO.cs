using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models
{
    public class UserDTO
    {
        public string? Profilename { get; set; }

        public string? Username { get; set; } // Login username, must be unique

        [MaxLength(150)]
        public string? Password { get; set; } // Hashed password

        public double Credits { get; set; }

        public string? Role { get; set; } // e.g., "Admin" or "User"
    }
}
