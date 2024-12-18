using Bobs_Racing.Hubs;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bobs_Racing.Services
{
    public class RaceService
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IAthleteRepository _athleteRepository;
        private readonly IRaceAthleteRepository _raceAthleteRepository;
        private readonly IBetRepository _betRepository;
        private readonly IUserRepository _userRepository;
        private readonly RaceSimulationService _simulationService;
        private readonly ILogger<RaceService> _logger;
        private readonly IHubContext<RaceSimulationHub> _hubContext;

        public RaceService(
            IRaceRepository raceRepository,
            IAthleteRepository athleteRepository,
            IRaceAthleteRepository raceAthleteRepository,
            IBetRepository betRepository,
            IUserRepository userRepository,
            RaceSimulationService simulationService,
            IHubContext<RaceSimulationHub> hubContext,
            ILogger<RaceService> logger)
        {
            _raceRepository = raceRepository;
            _athleteRepository = athleteRepository;
            _raceAthleteRepository = raceAthleteRepository;
            _betRepository = betRepository;
            _userRepository = userRepository;
            _simulationService = simulationService;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task StartRaceAsync(int raceId, CancellationToken cancellationToken)
        {
            var race = await _raceRepository.GetRaceByIdAsync(raceId);

            if (race == null || race.RaceAthletes == null)
            {
                _logger.LogWarning("Race {RaceId} or its athletes not found.", raceId);
                return;
            }

            // Prepare runners
            var raceAthleteMap = race.RaceAthletes.ToDictionary(ra => ra.AthleteId, ra => ra.RaceAthleteId);
            var athleteIds = race.RaceAthletes.Select(ra => ra.AthleteId).ToList();
            var athletes = await _athleteRepository.GetAthletesByIdsAsync(athleteIds);

            if (!athletes.Any())
            {
                _logger.LogWarning("No athletes found for race {RaceId}.", raceId);
                return;
            }

            var runners = athletes.Select(a => new Runner
            {
                Name = a.Name,
                Speed = 0.0,
                Position = 0.0,
                SlowestTime = a.SlowestTime,
                FastestTime = a.FastestTime,
                FinalPosition = 0,
                FinishTime = null,
                AthleteID = a.AthleteId,
                RaceAthleteID = raceAthleteMap[a.AthleteId]
            }).ToList();

            _simulationService.SetRunners(runners);
            await _simulationService.StartRace(cancellationToken);

            // Update raceAthlete results
            foreach (var runner in runners.Where(r => r.FinalPosition > 0))
            {
                await _raceAthleteRepository.UpdateRaceAthleteFinalPositionAsync(runner.RaceAthleteID, runner.FinalPosition);
                if (runner.FinishTime.HasValue)
                {
                    await _raceAthleteRepository.UpdateRaceAthleteFinishTimeAsync(runner.RaceAthleteID, runner.FinishTime.Value);
                }
            }

            // Determine winner
            var winner = runners.FirstOrDefault(r => r.FinalPosition == 1);
            if (winner == null) return;

            // Update bets
            var raceAthleteIds = race.RaceAthletes.Select(ra => ra.RaceAthleteId).ToList();
            var allBets = await _betRepository.GetBetsByRaceAthleteIdsAsync(raceAthleteIds);

            foreach (var bet in allBets)
            {
                bet.IsActive = false;
                if (bet.RaceAthleteId == winner.RaceAthleteID)
                {
                    var user = await _userRepository.GetUserByIdAsync(bet.UserId);
                    if (user != null)
                    {
                        user.Credits += bet.PotentialPayout;
                        await _userRepository.UpdateUserAsync(user.UserId, new UserDTO
                        {
                            Credits = user.Credits,
                            Profilename = user.Profilename,
                            Username = user.Username,
                            Role = user.Role
                        });
                        bet.IsWin = true;
                    }
                }
            }

            await _betRepository.UpdateBetsAsync(allBets);
            await _raceRepository.UpdateRaceIsFinishedAsync(raceId, true);

            // Broadcast final race results
            var result = new
            {
                RaceID = race.RaceId,
                IsFinished = true,
                Positions = runners.Select(r => new
                {
                    r.Name,
                    r.FinalPosition,
                    r.FinishTime
                })
            };

            await _hubContext.Clients.All.SendAsync("ReceiveRaceResults", result);
        }
    }
}
