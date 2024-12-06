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
        public string? Name { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }

        public int Credits { get; set; }

        [JsonIgnore]
        public List<Bet>? Bets { get; set; } = new List<Bet>();
        
    }
}
