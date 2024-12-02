using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // Navigation property for User

        // Composite Foreign Key: RaceId and AnimalId (via RaceAnimal)
        [Required]
        public int RaceId { get; set; }
        [Required]
        public int AnimalId { get; set; }

        [ForeignKey(nameof(RaceId) + "," + nameof(AnimalId))]
        public RaceAnimal RaceAnimal { get; set; } // Navigation property for RaceAnimal
    }
}
