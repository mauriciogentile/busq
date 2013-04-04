using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus.Messaging.Events
{
    public abstract class EventBase
    {
        public EventBase()
        {
            Timestamp = DateTime.Now;
        }

        public DateTimeOffset Timestamp { get; set; }
    }
}
