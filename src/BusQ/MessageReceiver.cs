using System;
using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ
{
    public class MessageReceiver<T> : IMessageReceiver<T>
    {
        readonly MessageReceiver _messageReceiverCore;

        public MessageReceiver(MessageReceiver receiverCore)
        {
            _messageReceiverCore = receiverCore;
        }

        public T Receive()
        {
            return _messageReceiverCore.Receive().GetBody<T>();
        }

        public T Receive(TimeSpan serverWaitTime)
        {
            return _messageReceiverCore.Receive(serverWaitTime).GetBody<T>();
        }

        public T Receive(long sequenceNumber)
        {
            return _messageReceiverCore.Receive(sequenceNumber).GetBody<T>();
        }

        public bool IsClosed
        {
            get { return _messageReceiverCore.IsClosed; }
        }

        public void Close()
        {
            if (!_messageReceiverCore.IsClosed)
                _messageReceiverCore.Close();
        }
    }
}
