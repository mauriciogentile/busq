using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public class ListenerSettings
    {
        public string QueueOrTopicPath { get; set; }
        public string SubscriptionName { get; set; }
        public ReceiveMode? ReceiveMode { get; set; }
        public TimeSpan? PollingTime { get; set; }
        public TimeSpan? PollingInterval { get; set; }
        public int? SequenceNumber { get; set; }
    }
}
