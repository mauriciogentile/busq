using Ringo.BusQ.ServiceBus.Messaging.Events;
using System;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public static class ListenerExtensions
    {
        public static Listener<T> OnError<T>(this Listener<T> listener, Action<ReceptionErrorEvent> action)
        {
            listener.EventBus.Subscribe(action);
            return listener;
        }

        public static Listener<T> OnStatusChanged<T>(this Listener<T> listener, Action<StatusChangedEvent> action)
        {
            listener.EventBus.Subscribe(action);
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, ConnectionSettings connSettings)
        {
            listener.ConnectionSettings = connSettings;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, string issuerName, string issuerKey, string serviceBusNamespace)
        {
            listener.ConnectionSettings = new ConnectionSettings
            {
                IssuerName = issuerName,
                IssuerSecretKey = issuerKey,
                ServiceBusNamespace = serviceBusNamespace
            };
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, string queueName)
        {
            listener.ListenerSettings = new ListenerSettings { QueueOrTopicPath = queueName };
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, string topicName, string subscriptionName)
        {
            listener.ListenerSettings = new ListenerSettings
            {
                QueueOrTopicPath = topicName,
                SubscriptionName = subscriptionName
            };
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, ListenerSettings settings)
        {
            listener.ListenerSettings = settings;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, IMessageReceiver receiver)
        {
            listener.MessageReceiver = receiver;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, IMessagingFactory messagingFactory)
        {
            listener.MessagingFactory = messagingFactory;
            return listener;
        }

        public static Listener<T> Set<T>(this Listener<T> listener, IMessageReceiver receiver, IMessagingFactory messagingFactory)
        {
            listener.MessageReceiver = receiver;
            listener.MessagingFactory = messagingFactory;
            return listener;
        }
    }
}
