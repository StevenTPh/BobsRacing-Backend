using Bobs_Racing.Hubs;
using Bobs_Racing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaceSimulationController : ControllerBase
    {
        private readonly RaceSimulationService _simulationService;
        private readonly List<Runner> _runners;

        public RaceSimulationController(RaceSimulationService simulationService)
        {
            _simulationService = simulationService;
            _runners = new List<Runner>
        {
            new Runner { Name = "Runner 1", Speed = 0, Position = 0, Acceleration = 0.2, ReactionTime = 0.1 },
            new Runner { Name = "Runner 2", Speed = 0, Position = 0, Acceleration = 0.25, ReactionTime = 0.2 },
            // Add more runners if needed
        };
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartRace()
        {
            var cts = new CancellationTokenSource();
            var task = _simulationService.StartRace(cts.Token);

            return Ok(new { message = "Race started!" });
        }
    }

}