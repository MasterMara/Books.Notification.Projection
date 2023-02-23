using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Books.Notification.Projection.Service;

public class MassTransitBusControlService : BackgroundService
{
    public MassTransitBusControlService(IBusControl busControl)
    {
        _busControl = busControl;
    }

    private readonly IBusControl _busControl;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _busControl.StartAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _busControl.StopAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}