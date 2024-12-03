using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class RaceAnimal
    {
        [Key, Column(Order = 0)] // Composite key
        public int RaceId { get; set; }
        [ForeignKey(nameof(RaceId))]
        [JsonIgnore]
        public Race? Race { get; set; }

        [Key, Column(Order = 1)] // Composite key
        public int AnimalId { get; set; }
        [ForeignKey(nameof(AnimalId))]
        [JsonIgnore]
        public Animal? Animal { get; set; }

        public int[] CheckpointSpeeds { get; set; } = new int[3]; // Array of speeds at checkpoints
        public int FinalPosition { get; set; } // Position in the race

        // Navigation property for Bets
        [JsonIgnore]
        public List<Bet> Bets { get; set; } = new List<Bet>();
    }
}
