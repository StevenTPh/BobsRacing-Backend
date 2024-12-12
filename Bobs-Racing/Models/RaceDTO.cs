using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class RaceDTO
    {
        //GET Races
        public int RaceId { get; set; }
        public DateTime Date { get; set; }

        public List<RaceAthlete>? RaceAthletes { get; set; } = new List<RaceAthlete>();
    }
}
