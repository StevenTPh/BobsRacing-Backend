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

        public RaceSimulationController(
            RaceSimulationService simulationService,
            IAthleteRepository athleteRepository,
            IHubContext<RaceSimulationHub> hubContext,
            IRaceAthleteRepository raceAthleteRepository)
        {
            _simulationService = simulationService;
            _athleteRepository = athleteRepository;
            _raceAthleteRepository = raceAthleteRepository;
            _simulationService = new RaceSimulationService(hubContext);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartRace([FromBody] List<int> raceAthleteIds, CancellationToken cancellationToken)
        {
            if (raceAthleteIds == null || !raceAthleteIds.Any())
            {
                return NotFound("No athlete IDs provided.");
            }

            var raceAthletes = await _raceAthleteRepository.GetAthletesByIdsAsyncList(raceAthleteIds);

            if (!raceAthletes.Any())
            {
                return NotFound("No race-athletes found for the given IDs.");
            }

            var raceAthleteMap = raceAthletes.ToDictionary(ra => ra.AthleteId, ra => ra.RaceAthleteId);

            var athleteIds = raceAthletes.Select(ra => ra.AthleteId).ToList();

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
                RaceID = raceAthletes.First().RaceId, // Assuming all runners belong to the same race
                Positions = positions
            };

            return Ok(result);
        }
    }

}