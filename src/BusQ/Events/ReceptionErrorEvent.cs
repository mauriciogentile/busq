using System;

namespace Ringo.BusQ.Events
{
    public class ReceptionErrorEvent : EventBase
    {
        public Exception Error { get; set; }
    }
}
