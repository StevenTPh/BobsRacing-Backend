using Bobs_Racing.Model;

namespace Bobs_Racing.Model
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
    }
}
