using Bobs_Racing.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bobs_Racing.Services
{
    public class RaceSchedulerService : IHostedService
    {
        private readonly ILogger<RaceSchedulerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RaceSchedulerService(ILogger<RaceSchedulerService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Race Scheduler Service starting...");
            CheckRacesForStart(null); // Initial call
            return Task.CompletedTask;
        }

        private void CheckRacesForStart(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var raceService = scope.ServiceProvider.GetRequiredService<RaceService>();
                var raceRepository = scope.ServiceProvider.GetRequiredService<IRaceRepository>();
                var currentTime = DateTime.Now;

                var races = raceRepository.GetAllRacesAsync().Result
                    .Where(r => !r.IsFinished && r.Date <= currentTime);

                foreach (var race in races)
                {
                    if (race.IsFinished == true)
                    {
                        // Skip race if it is already finished
                        continue;
                    }
                    _logger.LogInformation($"Starting Race ID: {race.RaceId}");
                    raceService.StartRaceAsync(race.RaceId, CancellationToken.None).Wait();
                }

                // Schedule the next run at the top of the next minute
                var nextRunTime = GetNextMinuteStart();
                var delay = nextRunTime - DateTime.Now;
                var delayMilliseconds = delay.TotalMilliseconds;

                if (delayMilliseconds > 0)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(delayMilliseconds)).ContinueWith(_ => CheckRacesForStart(null), TaskScheduler.Current);
                    _logger.LogInformation("Next check scheduled for: {Time}", nextRunTime);
                }
            }
        }

        private DateTime GetNextMinuteStart()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(1);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            // No need to manage a timer, thus no `Dispose` method required
            return Task.CompletedTask;
        }
    }
}
