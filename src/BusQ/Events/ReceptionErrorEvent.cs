using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus.Messaging.Events
{
    public class ReceptionErrorEvent : EventBase
    {
        public Exception Error { get; set; }
    }
}
