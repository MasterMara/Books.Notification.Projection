using Book.Events.V1.Book;
using Books.Notification.Projection.Logging;
using MassTransit;

namespace Books.Notification.Projection.Consumers.Book;

public class BookPublishedConsumer : BaseConsumer<Published>
{
    public BookPublishedConsumer(IConsoleLogger consoleLogger)
    {
        _consoleLogger = consoleLogger;
    }
    
    private readonly IConsoleLogger _consoleLogger;

    
    protected override async Task Consume(Published message, ConsumeContext consumeContext)
    {
        Console.WriteLine(message.BookNumber);
        // _consoleLogger.LogInformation($"Book:Created Consumer Started for BookNumber:{message.BookNumber}");
        
        //Todo: Go DomainService for Event Notification
        //_consoleLogger.LogInformation($"Book:Created Consumer Started for BookNumber:{message.BookNumber}");

        return;
    }
}