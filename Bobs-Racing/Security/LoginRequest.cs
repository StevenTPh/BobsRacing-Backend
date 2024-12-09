using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Security
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } // Used for login

        [Required]
        public string Password { get; set; }
    }
}
