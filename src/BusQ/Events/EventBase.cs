using System;

namespace Ringo.BusQ.Events
{
    public abstract class EventBase
    {
        protected EventBase()
        {
            Timestamp = DateTime.Now;
        }

        public DateTimeOffset Timestamp { get; set; }
    }
}
