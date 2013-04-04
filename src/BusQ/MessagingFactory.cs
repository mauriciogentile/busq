using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public class MessagingFactory : IMessagingFactory, IDisposable
    {
        readonly Microsoft.ServiceBus.Messaging.MessagingFactory messagingFactoryCore;
        bool disposed = false;

        public MessagingFactory(Uri serviceBusUri, TokenProvider tokenProvider)
        {
            this.messagingFactoryCore = Microsoft.ServiceBus.Messaging.MessagingFactory.Create(serviceBusUri, tokenProvider);
        }

        public IMessageReceiver CreateMessageReceiver(string entityPath, Microsoft.ServiceBus.Messaging.ReceiveMode receiveMode)
        {
            var receiverCore = this.messagingFactoryCore.CreateMessageReceiver(entityPath, receiveMode);
            return new MessageReceiver(receiverCore);
        }

        public bool IsClosed
        {
            get { return this.messagingFactoryCore.IsClosed; }
        }

        public void Close()
        {
            if (!this.messagingFactoryCore.IsClosed)
                this.messagingFactoryCore.Close();
        }

        #region Disposal

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Close();
            }

            disposed = true;
        }

        ~MessagingFactory()
        {
            Dispose(false);
        }

        #endregion
    }
}
