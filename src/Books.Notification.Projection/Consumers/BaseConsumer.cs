using MassTransit;

namespace Books.Notification.Projection.Consumers;

public class BaseConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
{
    
    
    public Task Consume(ConsumeContext<TMessage> context)
    {
        throw new NotImplementedException();
    }
}