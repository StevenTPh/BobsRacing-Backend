using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models
{
    public class RaceAnimal
    {
        [Key, Column(Order = 0)] // Composite key
        public int RaceId { get; set; }
        [ForeignKey(nameof(RaceId))]
        public Race? Race { get; set; }

        [Key, Column(Order = 1)] // Composite key
        public int AnimalId { get; set; }
        [ForeignKey(nameof(AnimalId))]
        public Animal? Animal { get; set; }

        public int[] CheckpointSpeeds { get; set; } = new int[3]; // Array of speeds at checkpoints
        public int FinalPosition { get; set; } // Position in the race
    }
}
