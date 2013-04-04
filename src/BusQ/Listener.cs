using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Ringo.BusQ.ServiceBus.Messaging.Events;
using System.Reactive.Linq;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public enum ListenerStatus
    {
        Running,
        NotStarted,
        Paused,
        Stopped
    }

    public class Listener : IDisposable, IListener
    {
        readonly static object syncLock;
        readonly static Dictionary<string, IMessageReceiver> receivers;
        bool disposed = false;

        internal IEventPublisher EventPublisher { get; set; }
        internal ListenerSettings ListenerSettings { get; set; }
        internal ConnectionSettings ConnectionSettings { get; set; }
        internal IMessageReceiver MessageReceiver { get; set; }
        internal IMessagingFactory MessagingFactory { get; set; }

        public ListenerStatus Status { get; private set; }

        static Listener()
        {
            receivers = new Dictionary<string, IMessageReceiver>();
            syncLock = new object();
        }

        public Listener()
            : this(null, null, null, null, null)
        {
        }

        public Listener(ConnectionSettings connSettings)
            : this(null, connSettings, null, null, null)
        {
        }

        internal Listener(ListenerSettings listenerSettings, ConnectionSettings connSettings, IMessageReceiver receiver, IEventPublisher eventPublisher, IMessagingFactory messagingFactory)
        {
            ListenerSettings = listenerSettings;
            ConnectionSettings = connSettings;
            MessageReceiver = receiver;
            EventPublisher = eventPublisher == null ? new EventPublisher() : eventPublisher;
            MessagingFactory = messagingFactory;
            Status = ListenerStatus.NotStarted;
        }

        public void Start()
        {
            ChangeStatus(ListenerStatus.Running);
            Task.Factory.StartNew(() => Listen());
        }

        public void Stop()
        {
            ChangeStatus(ListenerStatus.Stopped);
        }

        public void Pause()
        {
            ChangeStatus(ListenerStatus.Paused);
        }

        public void Resume()
        {
            Start();
        }

        protected virtual void ReceiveMessage()
        {
            BrokeredMessage message = null;

            var receiver = GetOrCreateMessageReceiver();

            if (ListenerSettings.PollingTime.HasValue)
            {
                message = receiver.Receive(ListenerSettings.PollingTime.Value);
            }
            else if (ListenerSettings.SequenceNumber.HasValue)
            {
                message = receiver.Receive(ListenerSettings.SequenceNumber.Value);
            }
            else
            {
                message = receiver.Receive();
            }

            if (message == null)
            {
                Thread.Sleep(ListenerSettings.PollingInterval.GetValueOrDefault(TimeSpan.FromSeconds(10)));
                Debug.WriteLine("Listener Sleeping");
            }
            else
            {
                Debug.WriteLine("Listener receiving message");
                EventPublisher.Publish(new MessageReceivedEvent() { Message = message });
            }
        }

        void Listen()
        {
            Debug.WriteLine("Listener Running");
            ListenInternal();
            Debug.WriteLine("Listener Stopped");
            Stop();
        }

        void ListenInternal()
        {
            while (Status == ListenerStatus.Running)
            {
                try
                {
                    ReceiveMessage();
                }
                catch (ThreadAbortException) { }
                catch (Exception exc)
                {
                    EventPublisher.Publish(new ReceptionErrorEvent() { Error = exc });
                }
            }
        }

        void ChangeStatus(ListenerStatus newStatus)
        {
            lock (syncLock)
            {
                if (Status == newStatus)
                {
                    return;
                }
                else if ((newStatus == ListenerStatus.Paused || newStatus == ListenerStatus.Stopped) && Status != ListenerStatus.Running)
                {
                    throw new InvalidOperationException("Listener is not running");
                }

                Status = newStatus;

                EventPublisher.Publish(new StatusChangedEvent() { NewStatus = Status });

                Debug.WriteLine("Listener status is '{0}'", Status);
            }
        }

        string GetEntityPath()
        {
            if (ListenerSettings == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(ListenerSettings.SubscriptionName))
            {
                return ListenerSettings.QueueOrTopicPath;
            }
            else
            {
                return EntityNameHelper.FormatSubscriptionPath(ListenerSettings.QueueOrTopicPath, ListenerSettings.SubscriptionName);
            }
        }

        IMessagingFactory GetOrCreateMessagingFactory()
        {
            if (MessagingFactory == null)
            {
                var tokenProvider = TokenProvider.CreateSharedSecretTokenProvider(ConnectionSettings.IssuerName, ConnectionSettings.IssuerSecretKey);
                var serviceUri = ServiceBusEnvironment.CreateServiceUri(ConnectionSettings.ServiceUriSchema, ConnectionSettings.ServiceBusNamespace, (ConnectionSettings.ServicePath == null ? string.Empty : ConnectionSettings.ServicePath));
                MessagingFactory = new MessagingFactory(serviceUri, tokenProvider);
            }

            return MessagingFactory;
        }

        IMessageReceiver GetOrCreateMessageReceiver()
        {
            lock (syncLock)
            {
                string entityPath = GetEntityPath();

                if (MessageReceiver != null && !MessageReceiver.IsClosed)
                {
                    return MessageReceiver;
                }

                var factory = GetOrCreateMessagingFactory();

                if (receivers.ContainsKey(entityPath) && receivers[entityPath].IsClosed)
                {
                    receivers.Remove(entityPath);
                }

                if (!receivers.ContainsKey(entityPath))
                {
                    var receiver = factory.CreateMessageReceiver(entityPath, ListenerSettings.ReceiveMode.GetValueOrDefault(ReceiveMode.ReceiveAndDelete));
                    receivers.Add(entityPath, receiver);
                }

                MessageReceiver = receivers[entityPath];

                return MessageReceiver;
            }
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
                MessagingFactory.Close();
            }

            disposed = true;
        }

        ~Listener()
        {
            Dispose(false);
        }

        #endregion
    }
}
