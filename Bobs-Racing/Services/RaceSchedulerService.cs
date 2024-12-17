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
    public class RaceSchedulerService : IHostedService, IDisposable
    {
        private readonly ILogger<RaceSchedulerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer = null;

        public RaceSchedulerService(ILogger<RaceSchedulerService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Race Scheduler Service starting...");

            // Align to the next minute and schedule the timer
            var nextRunTime = GetNextMinuteStart();
            var delay = nextRunTime - DateTime.Now;

            _timer = new Timer(CheckRacesForStart, null, delay, TimeSpan.FromMinutes(1));
            _logger.LogInformation("First check scheduled at {Time}.", nextRunTime);

            return Task.CompletedTask;
        }

        private DateTime GetNextMinuteStart()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(1);
        }

        private void CheckRacesForStart(object? state)
        {
            try
            {
                _logger.LogInformation("Checking races for start at: {Time}", DateTime.Now);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var raceRepository = scope.ServiceProvider.GetRequiredService<IRaceRepository>();

                    var currentTime = DateTime.Now;
                    var racesTask = raceRepository.GetAllRacesAsync();
                    racesTask.Wait();
                    var races = racesTask.Result;

                    foreach (var race in races.Where(r => !r.IsFinished && r.Date <= currentTime))
                    {
                        _logger.LogInformation("Starting Race ID: {RaceId} scheduled at {StartTime}", race.RaceId, race.Date);

                        // Implement race start logic here
                        // updateTask.Wait();

                        _logger.LogInformation("Race ID: {RaceId} has been marked as finished.", race.RaceId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking races for start.");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Race Scheduler Service stopping...");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
