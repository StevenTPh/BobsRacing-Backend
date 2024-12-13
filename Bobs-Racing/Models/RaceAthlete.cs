using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class RaceAthlete
    {
        [Key]
        public int RaceAthleteId { get; set; } // Surrogate key for this entity

        public int RaceId { get; set; }  // Foreign key for Race
        public int AthleteId { get; set; }  // Foreign key for Animal

        [JsonIgnore]
        [ForeignKey("RaceId")] 
        public Race? Race { get; set; }

        [JsonIgnore]
        [ForeignKey("AthleteId")]
        public Athlete? Athlete { get; set; }

        public int FinalPosition { get; set; } // Position in the race
        public double FinishTime { get; set; }

        // Navigation property for Bets
        [JsonIgnore]
        public List<Bet>? Bets { get; set; } = new List<Bet>();
    }
}