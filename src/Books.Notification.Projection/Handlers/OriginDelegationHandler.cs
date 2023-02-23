namespace Books.Notification.Projection.Handlers;

public class OriginDelegationHandler : DelegatingHandler
{
    private const string OriginKey = "Origin";
    private const string OriginValue = "Books.Notification.Projection";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.TryAddWithoutValidation(OriginKey, OriginValue);

        return await base.SendAsync(request, cancellationToken);
    }
}