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
        public int? Amount { get; set; } // Bet amount

        public int PotentialPayout { get; set; } // Potential payout

        public bool IsActive { get; set; }

        public int  UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public User? User { get; set; } // Navigation property for User

        public int RaceAthleteId { get; set; }  // Foreign Key for RaceAnimal
        [JsonIgnore]
        [ForeignKey("RaceAthleteId")]
        public RaceAthlete? RaceAthlete { get; set; }  // Navigation property
    }
}