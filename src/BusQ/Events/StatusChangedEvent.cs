using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus.Messaging.Events
{
    public class StatusChangedEvent : EventBase
    {
        public ListenerStatus NewStatus { get; set; }
    }
}
