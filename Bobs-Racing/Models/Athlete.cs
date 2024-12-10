using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class Athlete
    {
        [Key]
        public int AthleteId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double SlowestTime { get; set; }
        public double FastestTime { get; set; }
        [JsonIgnore]
        public List<RaceAthlete>? RaceAthletes { get; set; } = new List<RaceAthlete>();
    }
}
