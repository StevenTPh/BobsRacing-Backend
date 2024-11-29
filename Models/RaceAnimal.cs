namespace Bobs_Racing.Model
{
    public class RaceAnimal
    {
        public int RaceID { get; set; }
        public int AnimalID { get; set; }

        public List<int> CheckpointSpeed { get; set; } = new();
        public int FinalPosition { get; set; } 

        // Navigation properties
        public Race Race { get; set; }
        public Animal Animal { get; set; }
    }
}
