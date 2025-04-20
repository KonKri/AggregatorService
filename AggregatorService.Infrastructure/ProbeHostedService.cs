using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AggregatorService.Infrastructure;

internal class ProbeHostedService : BackgroundService
{
    private readonly ILogger<ProbeHostedService> _logger;

    public ProbeHostedService(ILogger<ProbeHostedService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // start of the service.
        _logger.LogInformation("Starting Probe Service...");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Running background task at: {Time}", DateTimeOffset.Now);

            // todo: add checks here. retrieve http calls with mediator, perform calls and log any anomalies.

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        // stop of the service.
        _logger.LogInformation("Stopping Probe Service...");
    }
}
