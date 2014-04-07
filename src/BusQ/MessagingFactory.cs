using System;
using Microsoft.ServiceBus;
using Ringo.BusQ.Util;

namespace Ringo.BusQ
{
    public class MessagingFactory : Disposable, IMessagingFactory
    {
        readonly Microsoft.ServiceBus.Messaging.MessagingFactory messagingFactoryCore;

        public MessagingFactory(Uri serviceBusUri, TokenProvider tokenProvider)
        {
            messagingFactoryCore = Microsoft.ServiceBus.Messaging.MessagingFactory.Create(serviceBusUri, tokenProvider);
            OnDispose = Close;
        }

        public IMessageReceiver<T> CreateMessageReceiver<T>(string entityPath, Microsoft.ServiceBus.Messaging.ReceiveMode receiveMode)
        {
            var receiverCore = messagingFactoryCore.CreateMessageReceiver(entityPath, receiveMode);
            return new MessageReceiver<T>(receiverCore);
        }

        public bool IsClosed
        {
            get { return messagingFactoryCore.IsClosed; }
        }

        public void Close()
        {
            if (!messagingFactoryCore.IsClosed)
                messagingFactoryCore.Close();
        }
    }
}
