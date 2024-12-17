using System.ComponentModel.DataAnnotations;

namespace Bobs_Racing.Models
{
    public class UserDTO
    {
        public string? Profilename { get; set; }

        public string? Username { get; set; } // Login username, must be unique

        [MaxLength(150)]
        public string? Password { get; set; } // Hashed password

        public double Credits { get; set; }

        public string? Role { get; set; } // e.g., "Admin" or "User"
    }

    public class UserWithBetsDTO
    {
        public int UserId { get; set; }
        public string Profilename { get; set; }
        public string Username { get; set; }
        public double Credits { get; set; }
        public string Role { get; set; }
        public List<BetDTO> Bets { get; set; }
    }
    public class BetDTO
    {
        public int BetId { get; set; }
        public double Amount { get; set; }
        public double PotentialPayout { get; set; }
        public bool IsActive { get; set; }
        public bool IsWin {  get; set; }
        public int RaceAthleteId { get; set; }
        public string AthleteName { get; set; }
    }
}
