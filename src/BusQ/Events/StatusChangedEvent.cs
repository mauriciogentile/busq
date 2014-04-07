namespace Ringo.BusQ.Events
{
    public class StatusChangedEvent : EventBase
    {
        public ListenerStatus NewStatus { get; set; }
    }
}
