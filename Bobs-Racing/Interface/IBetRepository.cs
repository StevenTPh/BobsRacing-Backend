﻿using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAllBetsAsync();
        Task<Bet> GetBetByIdAsync(int betId);
        Task<List<Bet>> GetBetsByRaceAthleteIdsAsync(List<int> raceAthleteIds);
        Task AddBetAsync(Bet bet);
        Task UpdateBetAsync(Bet bet);
        Task UpdateBetsAsync(IEnumerable<Bet> bets);
        Task DeleteBetAsync(int betId);

        // New method to validate the composite key in RaceAnimal
        Task<bool> ValidateRaceAthleteAsync(int raceAthleteId);
    }
}
