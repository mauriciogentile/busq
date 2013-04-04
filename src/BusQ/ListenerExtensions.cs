using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ringo.BusQ.ServiceBus.Messaging;
using Ringo.BusQ.ServiceBus.Messaging.Events;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    public static class ListenerExtensions
    {
        public static IObservable<TEvent> AsObservable<TEvent>(this Listener listener)
        {
            return listener.EventPublisher.GetEvent<TEvent>();
        }

        public static Listener Subscribe<TEvent>(this Listener listener, Action<TEvent> onNext) where TEvent : EventBase
        {
            listener.EventPublisher.GetEvent<TEvent>().Subscribe(onNext);
            return listener;
        }

        public static Listener Subscribe(this Listener listener, IEventPublisher publsher)
        {
            listener.EventPublisher = publsher;
            return listener;
        }

        public static Listener From(this Listener listener, ConnectionSettings connSettings)
        {
            listener.ConnectionSettings = connSettings;
            return listener;
        }

        public static Listener From(this Listener listener, string issuerName, string issuerKey, string serviceBusNamespace)
        {
            listener.ConnectionSettings = new ConnectionSettings() { IssuerName = issuerName, IssuerSecretKey = issuerKey, ServiceBusNamespace = serviceBusNamespace };
            return listener;
        }

        public static Listener Where(this Listener listener, string queueName)
        {
            listener.ListenerSettings = new ListenerSettings() { QueueOrTopicPath = queueName };
            return listener;
        }

        public static Listener Where(this Listener listener, string topicName, string subscriptionName)
        {
            listener.ListenerSettings = new ListenerSettings() { QueueOrTopicPath = topicName, SubscriptionName = subscriptionName };
            return listener;
        }

        public static Listener Where(this Listener listener, ListenerSettings settings)
        {
            listener.ListenerSettings = settings;
            return listener;
        }

        public static Listener Setup(this Listener listener, IMessageReceiver receiver)
        {
            listener.MessageReceiver = receiver;
            return listener;
        }

        public static Listener Setup(this Listener listener, IMessagingFactory messagingFactory)
        {
            listener.MessagingFactory = messagingFactory;
            return listener;
        }

        public static Listener Setup(this Listener listener, IMessageReceiver receiver, IMessagingFactory messagingFactory)
        {
            listener.MessageReceiver = receiver;
            listener.MessagingFactory = messagingFactory;
            return listener;
        }
    }
}
