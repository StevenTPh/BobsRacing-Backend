using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Security
{
    public class RegisterRequest
    {
        [Required]
        public string Profilename { get; set; } // User's profile name, e.g., "John Doe"

        [Required]
        public string Username { get; set; } // Unique login credential

        [Required]
        public string Password { get; set; }
    }
}
