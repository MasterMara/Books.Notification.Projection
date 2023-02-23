using Microsoft.Extensions.Logging;

namespace Books.Notification.Projection.Settings;

public class ConsoleLoggerSettings
{
    public LogLevel MinimumLogLevel { get; set; }
}