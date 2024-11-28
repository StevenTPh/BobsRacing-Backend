using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Model
{
    public class Race
    {
        [Key] // Specifies this property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int RaceId { get; set; }

        [MaxLength(100)] // Optional: Limits the length of the Result string to 100 characters
        public string Result { get; set; }

        public List<Animal> Animals { get; set; } = new List<Animal>();

        [Required] // Ensures the Checkpoints array must be provided
        public bool[] Checkpoints { get; set; } = new bool[3];
    }
}
