using Bobs_Racing.Hubs;
using Bobs_Racing.Interface;
using Bobs_Racing.Services;
using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

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

        public RaceSimulationController(
            RaceSimulationService simulationService,
            IAthleteRepository athleteRepository,
            IHubContext<RaceSimulationHub> hubContext,
            IRaceAthleteRepository raceAthleteRepository,
            IRaceRepository raceRepository)
        {
            _simulationService = simulationService;
            _athleteRepository = athleteRepository;
            _raceAthleteRepository = raceAthleteRepository;
            _simulationService = new RaceSimulationService(hubContext);
            _raceRepository = raceRepository;
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

            var raceAthleteMap = race.RaceAthletes.ToDictionary(ra => ra.AthleteId, ra => ra.RaceAthleteId);
            
            var athleteIds = race.RaceAthletes.Select(ra => ra.AthleteId).ToList();
            
            var athletes = await _athleteRepository.GetAthletesByIdsAsync(athleteIds);

            if (!athletes.Any())
            {
                return NotFound("No athletes found for the given IDs.");
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


            var positions = new Dictionary<int, object>();
            foreach (var runner in runners.OrderBy(r => r.FinalPosition).Where(r => r.FinalPosition > 0))
            {
                await _raceAthleteRepository.UpdateRaceAthleteFinalPositionAsync(runner.RaceAthleteID, runner.FinalPosition);

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

            var result = new
            {
                RaceID = race.RaceAthletes.First().RaceId, 
                Positions = positions
            };

            return Ok(result);
        }
    }

}