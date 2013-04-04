using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ringo.BusQ.ServiceBus.Messaging;
using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public interface IMessagingFactory
    {
        IMessageReceiver CreateMessageReceiver(string entityPath, ReceiveMode receiveMode);
        bool IsClosed { get; }
        void Close();
    }
}
