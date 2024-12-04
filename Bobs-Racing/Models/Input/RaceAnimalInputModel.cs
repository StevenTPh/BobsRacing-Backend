using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models.Input
{
    public class RaceAnimalInputModel
    {
        [Required]
        public int RaceId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "add at least 2 animals")]
        public List<int> AnimalIds { get; set; } = new List<int>(); 
    }
}
