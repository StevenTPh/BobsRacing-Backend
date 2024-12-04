using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
        [JsonIgnore]
        public List<RaceAnimal> RaceAnimals { get; set; } = new List<RaceAnimal>();
    }
}
