namespace Bobs_Racing.Model
{
    public class Race
    {
        public int RaceId { get; set; }
        public string Result { get; set; }
        public List<Animal> Animals { get; set; } = new List<Animal>();
        public bool Checkpoint1 { get; set; }
        public bool Checkpoint2 { get; set; }
        public bool Checkpoint3 { get; set; }
    }
}
