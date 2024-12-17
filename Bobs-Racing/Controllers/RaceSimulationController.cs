using Bobs_Racing.Hubs;
using Bobs_Racing.Interface;
using Bobs_Racing.Services;
using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Bobs_Racing.Repositories;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaceSimulationController : ControllerBase
    {
        private readonly RaceSimulationService _simulationService;
        private readonly IAthleteRepository _athleteRepository;
        private readonly IRaceAthleteRepository _raceAthleteRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly IHubContext<RaceSimulationHub> _hubContext;
        private IUserRepository _userRepository;
        private IBetRepository _betRepository;
        private readonly RaceService _raceService;

        public RaceSimulationController(
            RaceSimulationService simulationService,
            IAthleteRepository athleteRepository,
            IHubContext<RaceSimulationHub> hubContext,
            IRaceAthleteRepository raceAthleteRepository,
            IRaceRepository raceRepository,
            IUserRepository userRepository,
            IBetRepository betRepository,
            RaceService raceService
            )
        {
            _simulationService = simulationService;
            _athleteRepository = athleteRepository;
            _raceAthleteRepository = raceAthleteRepository;
            _simulationService = new RaceSimulationService(hubContext);
            _raceRepository = raceRepository;
            _hubContext = hubContext;
            _userRepository = userRepository;
            _betRepository = betRepository;
            _raceService = raceService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartRace(int raceId, CancellationToken cancellationToken)
        {

            var race = await _raceRepository.GetRaceByIdAsync(raceId);

            if (race == null)
            {
                return NotFound($"no race with id: {raceId}");
            }

            if (race.RaceAthletes == null)
            {
                return NotFound("no raceAthletes in race");
            }
            // athleteid of each raceAthlete becomes a key in the dict
            // raceAthlete id is the value inside the key
            var raceAthleteMap = race.RaceAthletes.ToDictionary(ra => ra.AthleteId, ra => ra.RaceAthleteId);
            
            // get just the athlete ids and put it into a list
            var athleteIds = race.RaceAthletes.Select(ra => ra.AthleteId).ToList();
            
            // finds each athlete in the athletes id list
            var athletes = await _athleteRepository.GetAthletesByIdsAsync(athleteIds);

            if (!athletes.Any())
            {
                return NotFound("No athletes found for the given IDs.");
            }

            // for each athlete create a runner object and set values.
            // raceAthlete id is found through the raceAthleteMap
            // creates a list of runner objects
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

            // sets runners for the race
            _simulationService.SetRunners(runners);

            // starts the race with the included runners
            // cancellationToken can be used to stop the executing the running task
            await _simulationService.StartRace(cancellationToken);

            // prepare a position dict
            var positions = new Dictionary<int, object>();

            // only continue to run runners with position = 0
            foreach (var runner in runners.OrderBy(r => r.FinalPosition).Where(r => r.FinalPosition > 0))
            {
                // Update the raceAthlete position
                await _raceAthleteRepository.UpdateRaceAthleteFinalPositionAsync(runner.RaceAthleteID, runner.FinalPosition);

                // Update the raceAthlete finish time
                if (runner.FinishTime.HasValue) // Ensure FinishTime is not null before updating
                {
                    await _raceAthleteRepository.UpdateRaceAthleteFinishTimeAsync(runner.RaceAthleteID, runner.FinishTime.Value);
                }

                positions[runner.FinalPosition] = new
                {
                    AthleteID = runner.AthleteID,
                    RaceAthleteID = runner.RaceAthleteID,
                    Name = runner.Name,
                    FinalPosition = runner.FinalPosition,
                    FinishTime = runner.FinishTime
                };

                Console.WriteLine($"Name: {runner.Name}, Final: {runner.FinalPosition}, Speed: {runner.Speed}, SlowestTime: {runner.SlowestTime}, FastestTime: {runner.FastestTime}, AthleteID: {runner.AthleteID}, RaceAthleteID: {runner.RaceAthleteID}");
            }

            // winner
            var winner = runners.FirstOrDefault(r => r.FinalPosition == 1);
            if (winner == null)
            {
                return BadRequest("no winner in race.");
            }

            // find all bets
            var raceAthleteIds = race.RaceAthletes.Select(ra => ra.RaceAthleteId).ToList();
            var allBets = await _betRepository.GetBetsByRaceAthleteIdsAsync(raceAthleteIds);

            if (allBets.Any())
            {
                foreach (var bet in allBets)
                {
                    // Mark all bets as inactive
                    bet.IsActive = false;

                    // For winning bets, pay out and mark as paid
                    if (bet.RaceAthleteId == winner.RaceAthleteID)
                    {
                        var user = await _userRepository.GetUserByIdAsync(bet.UserId);
                        if (user != null)
                        {
                            user.Credits += bet.PotentialPayout;

                            var userDto = new UserDTO
                            {
                                Credits = user.Credits,
                                Profilename = user.Profilename,
                                Username = user.Username,
                                Role = user.Role,
                            };
                            bet.IsWin = true;

                            await _userRepository.UpdateUserAsync(user.UserId, userDto);
                        }
                    }
                }

                await _betRepository.UpdateBetsAsync(allBets);
            }

            await _raceRepository.UpdateRaceIsFinishedAsync(raceId, true);

            // sets the result return
            var result = new
            {
                RaceID = race.RaceAthletes.First().RaceId,
                IsFinished = race.IsFinished,
                Positions = positions
            };

            // Broadcast the results to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveRaceResults", result);

            await _raceService.StartRaceAsync(raceId, cancellationToken);
            return Ok($"Race {raceId} started successfully.");
        }

        [HttpGet("results/{raceId}")]
        public async Task<IActionResult> GetRaceResults(int raceId)
        {
            var race = await _raceRepository.GetRaceByIdAsync(raceId);
            if (race == null || !race.IsFinished)
            {
                return NotFound("Race not found or not finished.");
            }

            var results = new
            {
                RaceID = race.RaceId,
                IsFinished = race.IsFinished,
                Positions = race.RaceAthletes.OrderBy(ra => ra.FinalPosition).Select(ra => new
                {
                    AthleteID = ra.AthleteId,
                    RaceAthleteID = ra.RaceAthleteId,
                    Name = ra.Athlete.Name,
                    FinalPosition = ra.FinalPosition,
                    FinishTime = ra.FinishTime
                })
            };

            return Ok(results);
        }

    }

}