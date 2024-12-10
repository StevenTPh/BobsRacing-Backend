namespace Bobs_Racing.Services
{
    using Microsoft.AspNetCore.SignalR;
    using Bobs_Racing.Hubs;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Bobs_Racing.Data;

    public class RaceSimulationService
    {
        private const double TimeStep = 0.11; // 50ms
        private const double TrackLength = 100.0; // 100 meters
        private List<Runner> _runners;
        private readonly IHubContext<RaceSimulationHub> _hubContext;

        public RaceSimulationService(IHubContext<RaceSimulationHub> hubContext)
        {
            _runners = new List<Runner>();
            _hubContext = hubContext;

        }

        public void SetRunners(List<Runner> runners)
        {
            _runners = runners;
        }

        public async Task StartRace(CancellationToken cancellationToken)
        {
            bool raceComplete = false;
            double timeElapsed = 0;
            int finishOrder = 1;

            while (!raceComplete && !cancellationToken.IsCancellationRequested)
            {
                raceComplete = true;
                Random random = new Random();

                foreach (var runner in _runners)
                {
                    if (runner.FinalPosition != 0)
                        continue;

                    if (timeElapsed >= 0)
                    {
                        runner.Speed = 100 / runner.SlowestTime + (random.NextDouble() * (runner.SlowestTime - runner.FastestTime));
                        runner.Position += runner.Speed * TimeStep;
                    }




                    if (runner.Position >= TrackLength && runner.FinalPosition == 0)
                    {
                        runner.FinalPosition = finishOrder++;
                    }
                    else if (runner.Position < TrackLength)
                    {
                        raceComplete = false;
                    }
                }

                // check if race is running
                Console.WriteLine($"Time: {timeElapsed}, Runners: {string.Join(", ", _runners.Select(r => $"{r.Name}: {r.Position:F2}m (Pos: {r.FinalPosition})"))}");

                await _hubContext.Clients.All.SendAsync("ReceiveRaceUpdate", _runners);

                    await Task.Delay((int)(TimeStep * 1000), cancellationToken);
                    timeElapsed += TimeStep;
            
                //Console.WriteLine("Race Complete!");
            }
            Console.WriteLine("Race Complete!");

        }

    }
}