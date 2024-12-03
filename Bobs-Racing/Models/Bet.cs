using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; } // Primary Key for the Bet table

        [Required]
        public int Amount { get; set; } // Bet amount

        [Required]
        public DateTime Date { get; set; } // Date of the bet

        public int PotentialPayout { get; set; } // Potential payout

        // Foreign Key: UserId
        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // Navigation property for User

        // Composite Foreign Key: RaceId and AnimalId (via RaceAnimal)
        [Required]
        [JsonIgnore]
        public int RaceId { get; set; }
        [Required]
        [JsonIgnore]
        public int AnimalId { get; set; }

        [ForeignKey(nameof(RaceId) + "," + nameof(AnimalId))]
        public RaceAnimal RaceAnimal { get; set; } // Navigation property for RaceAnimal
    }
}
