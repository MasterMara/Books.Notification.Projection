using System.Diagnostics;
using MassTransit;

namespace Books.Notification.Projection;

public static class CorrelationManager
{
    //Extension Method of ConsumeContext 
    public static void SetConsumeActivityId(this ConsumeContext context)
    {
        if (!context.CorrelationId.HasValue || context.CorrelationId.Value == Guid.Empty)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        }
        else
        {
            Trace.CorrelationManager.ActivityId = context.CorrelationId.Value;
        }
    }
    
}