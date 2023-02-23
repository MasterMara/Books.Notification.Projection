using Books.Events.V1.Book;
using Books.Notification.Projection.Logging;
using MassTransit;

namespace Books.Notification.Projection.Consumers.Book;

public class BookCreatedConsumer : BaseConsumer<Created>
{
    public BookCreatedConsumer(IConsoleLogger consoleLogger)
    {
        _consoleLogger = consoleLogger;
    }
    
    private readonly IConsoleLogger _consoleLogger;

    
    protected override async Task Consume(Created message, ConsumeContext consumeContext)
    {
        _consoleLogger.LogInformation($"Book:Created Consumer Started for BookNumber:{message.BookNumber}");
        
        //Todo: Go DomainService for Event Notification
        _consoleLogger.LogInformation($"Book:Created Consumer Started for BookNumber:{message.BookNumber}");

        return;
    }
}