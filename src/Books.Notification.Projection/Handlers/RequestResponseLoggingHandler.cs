using System.Diagnostics;
using Books.Notification.Projection.Logging;

namespace Books.Notification.Projection.Handlers;

public class RequestResponseLoggingHandler : DelegatingHandler
{
    private readonly IConsoleLogger _consoleLogger;

    public RequestResponseLoggingHandler(IConsoleLogger consoleLogger)
    {
        _consoleLogger = consoleLogger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        var requestString = request.Content != null ? await request.Content.ReadAsStringAsync() : null;

        var hostName = request.RequestUri?.Host;

        var absolutePath = request.RequestUri?.AbsolutePath;

        _consoleLogger.LogInformation("Starting request", httpMethod: request.Method, url: absolutePath,
            requestBody: requestString, hostName: hostName);

        var response = await base.SendAsync(request, cancellationToken);

        sw.Stop();

        var responseString = await response.Content.ReadAsStringAsync();

        var responseContentLimitInKb = 100;

        if (responseString.Length > 1024 * responseContentLimitInKb) //drop if bigger than 5kb
        {
            responseString =
                $"Response content dropped due length is {responseString.Length / 1024}kb which bigger than {responseContentLimitInKb}kb";
        }

        _consoleLogger.LogInformation($"Finished request",
            httpMethod: request.Method,
            url: absolutePath,
            duration: sw.ElapsedMilliseconds,
            httpStatusCode: response.StatusCode,
            requestBody: requestString,
            responseBody: responseString,
            hostName: hostName);

        return response;
    }
}