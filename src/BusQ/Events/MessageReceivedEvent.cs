namespace Ringo.BusQ.Events
{
    public class MessageReceivedEvent<T> : EventBase
    {
        public T Message { get; set; }
    }
}
