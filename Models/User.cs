using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }
        
        public int Credits { get; set; }
        public string Password { get; set; }

        public User()
        {
            
        }

        public User(int id, string name, int credits, string password)
        {
            this.Id = id;
            this.Name = name;
            this.Credits = credits;
            this.Password = password;
        }
   

        public User(int id)
        {
            this.Id = id;
        }
        
    }
}
