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

        //[Required]
        public int Amount { get; set; } // Bet amount

        //[Required]
        public DateTime Date { get; set; } // Date of the bet

        public int PotentialPayout { get; set; } // Potential payout

        public int  UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; } // Navigation property for User

        public int RaceAnimalId { get; set; }  // Foreign Key for RaceAnimal
        [JsonIgnore]
        [ForeignKey("RaceAnimalId")]
        public RaceAnimal RaceAnimal { get; set; }  // Navigation property
    }
}