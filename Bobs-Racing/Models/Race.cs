using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class Race //OlympicGame
    {
        //POST, PUT, DELETE Race
        [Key]
        public int RaceId { get; set; }
        public DateTime Date { get; set; }
        public bool IsFinished { get; set; }

        [JsonIgnore]
        public List<RaceAthlete>? RaceAthletes { get; set; } = new List<RaceAthlete>();

    }
}
