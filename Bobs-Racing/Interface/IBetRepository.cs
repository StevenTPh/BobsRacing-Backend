using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAllBetsAsync();
        Task<Bet> GetBetByIdAsync(int betId);
        Task AddBetAsync(Bet bet);
        Task UpdateBetAsync(Bet bet);
        Task DeleteBetAsync(int betId);

        // New method to validate the composite key in RaceAnimal
        Task<bool> ValidateRaceAthleteAsync(int raceAthleteId);
    }
}
