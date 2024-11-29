
namespace Bobs_Racing.Model
{
    public class Race
    {
        public int RaceId { get; set; }
        public List<int> Rankings { get; set; } = new();
        public DateTime StartTime { get; set; }

        public ICollection<RaceAnimal> RaceAnimals { get; set; } = new List<RaceAnimals>();
    }
}
