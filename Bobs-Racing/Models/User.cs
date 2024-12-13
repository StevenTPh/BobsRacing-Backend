using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Profilename { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; } // Login username, must be unique

        [Required]
        [MaxLength(150)]
        public string? Password { get; set; } // Hashed password

        public double Credits { get; set; }

        public string Role { get; set; } // e.g., "Admin" or "User"

        [JsonIgnore]
        public List<Bet>? Bets { get; set; } = new List<Bet>();
        
    }
}
