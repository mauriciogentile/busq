using System;
using Ringo.BusQ.Events;

namespace Ringo.BusQ
{
    public static class ListenerExtensions
    {
        public static Listener<T> OnError<T>(this Listener<T> listener, Action<ReceptionErrorEvent> action) where T : class
        {
            listener.EventBus.Subscribe(action);
            return listener;
        }

        public static Listener<T> OnStatusChanged<T>(this Listener<T> listener, Action<StatusChangedEvent> action) where T : class
        {
            listener.EventBus.Subscribe(action);
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, ConnectionSettings connSettings) where T : class
        {
            listener.ConnectionSettings = connSettings;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, string issuerName, string issuerKey, string serviceBusNamespace) where T : class
        {
            listener.ConnectionSettings = new ConnectionSettings
            {
                IssuerName = issuerName,
                IssuerSecretKey = issuerKey,
                ServiceBusNamespace = serviceBusNamespace
            };
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, string queueName) where T : class
        {
            listener.ListenerSettings = new ListenerSettings { QueueOrTopicPath = queueName };
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, string topicName, string subscriptionName) where T : class
        {
            listener.ListenerSettings = new ListenerSettings
            {
                QueueOrTopicPath = topicName,
                SubscriptionName = subscriptionName
            };
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, ListenerSettings settings) where T : class
        {
            listener.ListenerSettings = settings;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, IMessageReceiver<T> receiver) where T : class
        {
            listener.MessageReceiver = receiver;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, IMessagingFactory messagingFactory) where T : class
        {
            listener.MessagingFactory = messagingFactory;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, IMessageReceiver<T> receiver, IMessagingFactory messagingFactory) where T : class
        {
            listener.MessageReceiver = receiver;
            listener.MessagingFactory = messagingFactory;
            return listener;
        }
    }
}
