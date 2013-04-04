using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public interface IMessageReceiver
    {
        BrokeredMessage Receive();
        BrokeredMessage Receive(TimeSpan pollingTime);
        BrokeredMessage Receive(long longSeuence);
        bool IsClosed { get; }
        void Close();
    }
}
