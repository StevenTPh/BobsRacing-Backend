using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bobs_Racing.Models
{
    public class Race
    {
        [Key]
        public int RaceId { get; set; }
        [JsonIgnore]
        public List<RaceAnimal>? RaceAnimals { get; set; } = new List<RaceAnimal>();
        public DateTime Date { get; set; }
        // This property holds the Rankings as a string in the database
        [NotMapped]
        public int[] Rankings { get; set; } = new int[3];
        /*
        public string RankingsString { get; set; } // Store as a comma-separated string in the database

        [NotMapped]
        public List<int> Rankings
        {
            get => RankingsString?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
            set => RankingsString = string.Join(",", value);
        } */
    }
}
