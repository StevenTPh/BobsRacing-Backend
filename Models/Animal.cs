using Bobs_Racing.Model;

namespace Bobs_Racing.Model
{
    public class Animal
    {
        public int AnimalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }

        public ICollection<RaceAnimal> RaceAnimals { get; set; } = new List<RaceAnimal>();
    }
}
