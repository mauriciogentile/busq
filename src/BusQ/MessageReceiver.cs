using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public class MessageReceiver : IMessageReceiver
    {
        readonly Microsoft.ServiceBus.Messaging.MessageReceiver messageReceiverCore;

        public MessageReceiver(Microsoft.ServiceBus.Messaging.MessageReceiver receiverCore)
        {
            messageReceiverCore = receiverCore;
        }

        public BrokeredMessage Receive()
        {
            return messageReceiverCore.Receive();
        }

        public BrokeredMessage Receive(TimeSpan serverWaitTime)
        {
            return messageReceiverCore.Receive(serverWaitTime);
        }

        public BrokeredMessage Receive(long sequenceNumber)
        {
            return messageReceiverCore.Receive(sequenceNumber);
        }

        public bool IsClosed
        {
            get { return messageReceiverCore.IsClosed; }
        }

        public void Close()
        {
            if (!messageReceiverCore.IsClosed)
                messageReceiverCore.Close();
        }
    }
}
