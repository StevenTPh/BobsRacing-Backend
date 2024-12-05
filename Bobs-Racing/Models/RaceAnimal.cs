using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class RaceAnimal
    {
        [Key]
        public int RaceAnimalId { get; set; } // Surrogate key for this entity

        [ForeignKey("RaceId")]
        public int RaceId { get; set; }  // Foreign key for Race
        [ForeignKey("AnimalId")]
        public int AnimalId { get; set; }  // Foreign key for Animal

        [JsonIgnore]
        public Race Race { get; set; }

        //[ForeignKey(nameof(AnimalId))]
        [JsonIgnore]
        public Animal Animal { get; set; }

        /*
        // CheckpointSpeeds string for database storage
        public string CheckpointSpeedsString { get; set; }

        [NotMapped]
        public List<int> CheckpointSpeeds
        {
            get => CheckpointSpeedsString?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
            set => CheckpointSpeedsString = string.Join(",", value);
        } */
        public int[] CheckpointSpeeds { get; set; } = new int[3];

        public int FinalPosition { get; set; } // Position in the race

        // Navigation property for Bets
        [JsonIgnore]
        public List<Bet> Bets { get; set; } = new List<Bet>();
    }
}