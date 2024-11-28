namespace Bobs_Racing.Model
{
    public class Race
    {
        public int RaceId { get; set; }
        public string Result { get; set; }
        public List<Animal> Animals { get; set; } = new List<Animal>();
    }
}
