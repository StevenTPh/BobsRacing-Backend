using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models.Input
{
    public class RaceAthleteInputModel
    {
        [Required]
        public int RaceId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "add at least 2 athletes")]
        public List<int> AthleteIds { get; set; } = new List<int>(); 
    }
}
