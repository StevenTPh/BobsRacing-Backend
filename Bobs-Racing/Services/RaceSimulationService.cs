namespace Bobs_Racing.Services
{
    using Microsoft.AspNetCore.SignalR;
    using Bobs_Racing.Hubs;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class RaceSimulationService
    {
        private const double TimeStep = 0.05; // 50ms
        private const double TrackLength = 100.0; // 100 meters
        private readonly List<Runner> _runners;
        private readonly IHubContext<RaceSimulationHub> _hubContext;

        public RaceSimulationService(List<Runner> runners, IHubContext<RaceSimulationHub> hubContext)
        {
            _runners = runners;
            _hubContext = hubContext;
        }
        public async Task StartRace(CancellationToken cancellationToken)
        {
            bool raceComplete = false;
            double timeElapsed = 0;

            while (!raceComplete && !cancellationToken.IsCancellationRequested)
            {
                raceComplete = true;

                foreach (var runner in _runners)
                {
                    if (timeElapsed >= runner.ReactionTime)
                    {
                        runner.Speed += runner.Acceleration * TimeStep;
                        runner.Position += runner.Speed * TimeStep;
                    }

                    if (runner.Position < TrackLength)
                    {

                        raceComplete = false;

                    }
                }

                // check if race is running
                Console.WriteLine($"Time: {timeElapsed}, Runners: {string.Join(", ", _runners.Select(r => $"{r.Name}: {r.Position:F2}m"))}");

                await _hubContext.Clients.All.SendAsync("ReceiveRaceUpdate", _runners);

                await Task.Delay((int)(TimeStep * 1000), cancellationToken);
                timeElapsed += TimeStep;
            }
            Console.WriteLine("Race Complete!");
        }
    }

}