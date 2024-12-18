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