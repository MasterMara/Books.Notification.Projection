using System.Diagnostics;

namespace Books.Notification.Projection.Handlers;

public class CorrelationIdHandler : DelegatingHandler
{
    private const string CorrelationIdKey = "X-Correlation-ID";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.TryAddWithoutValidation(CorrelationIdKey, Trace.CorrelationManager.ActivityId.ToString());
        
        return await base.SendAsync(request, cancellationToken);
    }
}