using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing
{
    public class Bet
    {
        [Key]
        public int BetID { get; set; } // Primary Key for the Bet table

        [Required]
        public int Amount { get; set; } // Bet amount

        [Required]
        public DateTime Date { get; set; } // Date of the bet

        public int PotentialPayout { get; set; } // Potential payout

        // Composite Foreign Key: RaceID and AnimalID
        [Required]
        public int RaceID { get; set; }
        public Race Race { get; set; } // Navigation property for Race

        [Required]
        public int AnimalID { get; set; }
        public Animal Animal { get; set; } // Navigation property for Animal
    }
}
