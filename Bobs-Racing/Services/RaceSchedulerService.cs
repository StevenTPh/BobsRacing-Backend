namespace Bobs_Racing.Services
{
    public class RaceSchedulerService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<RaceSchedulerService> _logger;
        private Timer? _timer = null;

        public RaceSchedulerService(ILogger<RaceSchedulerService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            //start timer?
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }
        // Task (implement signalR and startrace logic here)
        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            // stop timer
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //dispose timer
            _timer?.Dispose();
        }
    }
}
