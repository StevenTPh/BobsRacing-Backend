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

        public RaceSimulationController(
            RaceSimulationService simulationService,
            IAthleteRepository athleteRepository,
            IHubContext<RaceSimulationHub> hubContext)
        {
            _simulationService = simulationService;
            _athleteRepository = athleteRepository;
            _simulationService = new RaceSimulationService(hubContext);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartRace([FromBody] List<int> athleteIds, CancellationToken cancellationToken)
        {
            if (athleteIds == null || !athleteIds.Any())
            {
                return NotFound("No athlete IDs provided.");
            }

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
                FastestTime = a.FastestTime
            }).ToList();

            _simulationService.SetRunners(runners);

            await _simulationService.StartRace(cancellationToken);

            return Ok(new { message = "Race started!" });
        }
    }

}