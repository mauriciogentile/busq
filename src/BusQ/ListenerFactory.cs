using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ringo.BusQ.ServiceBus.Messaging;
using Ringo.BusQ.ServiceBus;
using Ringo.BusQ.ServiceBus.Messaging.Events;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    internal static class ListenerFactory
    {
        public static Listener Create(string queueName, ConnectionSettings connSettings, IEventPublisher eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = queueName
            };

            return Create(settings, connSettings, eventPublisher);
        }

        public static Listener Create(string queueName, ConnectionSettings connSettings, IMessageReceiver receiver, IEventPublisher eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = queueName
            };

            return Create(settings, connSettings, receiver, eventPublisher);
        }

        public static Listener Create(string topicName, string subscriptionName, ConnectionSettings connSettings, IEventPublisher eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = topicName,
                SubscriptionName = subscriptionName
            };

            return Create(settings, connSettings, eventPublisher);
        }

        public static Listener Create(string topicName, string subscriptionName, string issuerName, string issuerKey, string serviceBusNamespace, IEventPublisher eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = topicName,
                SubscriptionName = subscriptionName
            };

            var connSettings = new ConnectionSettings()
            {
                IssuerName = issuerName,
                IssuerSecretKey = issuerKey,
                ServiceBusNamespace = serviceBusNamespace
            };

            return Create(settings, connSettings, eventPublisher);
        }

        public static Listener Create(string queueName, string issuerName, string issuerKey, string serviceBusNamespace, IEventPublisher eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = queueName
            };

            var connSettings = new ConnectionSettings()
            {
                IssuerName = issuerName,
                IssuerSecretKey = issuerKey,
                ServiceBusNamespace = serviceBusNamespace
            };

            return Create(settings, connSettings, eventPublisher);
        }

        public static Listener Create(ListenerSettings settings, ConnectionSettings connSettings, IEventPublisher eventPublisher)
        {
            return new Listener(settings, connSettings, null, eventPublisher, null);
        }

        public static Listener Create(ListenerSettings settings, ConnectionSettings connSettings, IMessageReceiver receiver, IEventPublisher eventPublisher = null)
        {
            return new Listener(settings, connSettings, receiver, eventPublisher, null);
        }

        public static Listener Create(ListenerSettings settings, ConnectionSettings connSettings, IMessageReceiver receiver, IEventPublisher eventPublisher, IMessagingFactory messagingFactory)
        {
            return new Listener(settings, connSettings, receiver, eventPublisher, messagingFactory);
        }
    }
}
