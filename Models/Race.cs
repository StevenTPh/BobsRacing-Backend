
namespace Bobs_Racing.Model
{
    public class Race
    {
        public int RaceId { get; set; }
        public List<int> Rankings { get; set; } = new();
        public DateTime StartTime { get; set; }

        public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    }
}
