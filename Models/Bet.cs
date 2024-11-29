namespace Bobs_Racing.Model
{
    public class Bet
    {
        public int BetId { get; set; }
        public int RaceID { get; set; }
        public int AnimalID { get; set; }

        public RaceAnimal RaceAnimal { get; set; }
    }
}
