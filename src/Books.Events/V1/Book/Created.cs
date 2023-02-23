using Books.Events.Abstract;
using Books.Events.Common;
using Books.Events.V1.Book.Props;

namespace Books.Events.V1.Book;

public class Created : IBookEvent
{
    public string Id { get; set; }
    public string BookNumber { get; set; }
    public string BookName { get; set; }
    public Writer Writer { get; set; }
    public Money TotalAmount { get; set; }
}