using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class Athlete
    {
        [Key]
        public int AthleteId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; }
        public int LowestTime { get; set; }
        public int FastestTime { get; set; }
        [JsonIgnore]
        public List<RaceAthlete>? RaceAthletes { get; set; } = new List<RaceAthlete>();
    }
}
