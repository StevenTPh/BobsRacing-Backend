using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Security
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } // Used for login

        [Required]
        [MaxLength(150)]
        public string Password { get; set; }
    }
}
