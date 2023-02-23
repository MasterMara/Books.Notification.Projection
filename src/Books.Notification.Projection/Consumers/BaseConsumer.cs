using MassTransit;

namespace Books.Notification.Projection.Consumers;

public abstract class BaseConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
{
    protected abstract Task Consume(TMessage context, ConsumeContext consumeContext);
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        context.SetConsumeActivityId();

        context.TryGetPayload(out ConsumeContext consumeContext);

        await Consume(context.Message, consumeContext);
    }
}