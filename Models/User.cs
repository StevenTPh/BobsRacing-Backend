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
        public string Name { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public int Credits { get; set; }

        [JsonIgnore]
        public List<Bet> Bets { get; set; } = new List<Bet>();

        public User()
        {
            
        }

        public User(int id, string name, int credits, string password)
        {
            this.UserId = id;
            this.Name = name;
            this.Credits = credits;
            this.Password = password;
        }
   

        public User(int id)
        {
            this.UserId = id;
        }
        
    }
}
