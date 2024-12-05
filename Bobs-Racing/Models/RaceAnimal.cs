using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class RaceAnimal
    {
        [Key]
        public int RaceAnimalId { get; set; } // Surrogate key for this entity

        public int RaceId { get; set; }  // Foreign key for Race
        public int AnimalId { get; set; }  // Foreign key for Animal

        [JsonIgnore]
        [ForeignKey("RaceId")] 
        public Race? Race { get; set; }

        [JsonIgnore]
        [ForeignKey("AnimalId")]
        public Animal? Animal { get; set; }

        public int[] CheckpointSpeeds { get; set; } = new int[3]; // Speeds in different checkpoints in the race

        public int FinalPosition { get; set; } // Position in the race

        // Navigation property for Bets
        [JsonIgnore]
        public List<Bet>? Bets { get; set; } = new List<Bet>();
    }
}