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
            _timer = new Timer(CheckRacesForStart, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
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
                    if(race.IsFinished == true)
                    {
                        //skip race if it is already finished
                        continue;
                    }
                    _logger.LogInformation($"Starting Race ID: {race.RaceId}");
                    raceService.StartRaceAsync(race.RaceId, CancellationToken.None).Wait();
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
