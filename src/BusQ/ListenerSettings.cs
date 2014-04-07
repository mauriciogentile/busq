using System;
using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ
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
