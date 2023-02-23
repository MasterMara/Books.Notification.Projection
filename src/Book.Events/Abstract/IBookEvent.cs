using Books.Events.Abstract.Base;

namespace Books.Events.Abstract;

public interface IBookEvent : IEvent
{
    public string BookNumber { get; set; }
}