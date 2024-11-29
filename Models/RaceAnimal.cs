namespace Bobs_Racing.Models
{
    public class RaceAnimal
    {
        public int RaceId { get; set; }
        public Race Race { get; set; }

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        public int[] CheckpointSpeeds { get; set; } = new int[3]; // Array of speeds at checkpoints
        public int FinalPosition { get; set; } // Position in the race
    }
}
