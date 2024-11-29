using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }
        public int Credits { get; set; }

        public User(string name, int credits)
        {
            this.Name = name;
            this.Credits = credits;
        }
    }
}
