using Ringo.BusQ.Events;

namespace Ringo.BusQ
{
    internal static class ListenerFactory<T> where T : class
    {
        public static Listener<T> Create(string queueName, ConnectionSettings connSettings,
            IEventBus eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = queueName
            };

            return Create(settings, connSettings, eventPublisher);
        }

        public static Listener<T> Create(string queueName, ConnectionSettings connSettings,
            IMessageReceiver<T> receiver, IEventBus eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = queueName
            };

            return Create(settings, connSettings, receiver, eventPublisher);
        }

        public static Listener<T> Create(string topicName, string subscriptionName, ConnectionSettings connSettings,
            IEventBus eventPublisher = null)
        {
            var settings = new ListenerSettings()
            {
                QueueOrTopicPath = topicName,
                SubscriptionName = subscriptionName
            };

            return Create(settings, connSettings, eventPublisher);
        }

        public static Listener<T> Create(string topicName, string subscriptionName, string issuerName,
            string issuerKey, string serviceBusNamespace, IEventBus eventPublisher = null)
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

        public static Listener<T> Create(string queueName, string issuerName, string issuerKey, string serviceBusNamespace, IEventBus eventPublisher = null)
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

        public static Listener<T> Create(ListenerSettings settings, ConnectionSettings connSettings, IEventBus eventPublisher)
        {
            return new Listener<T>(settings, connSettings, null, eventPublisher, null);
        }

        public static Listener<T> Create(ListenerSettings settings, ConnectionSettings connSettings, IMessageReceiver<T> receiver, IEventBus eventPublisher = null)
        {
            return new Listener<T>(settings, connSettings, receiver, eventPublisher, null);
        }

        public static Listener<T> Create(ListenerSettings settings, ConnectionSettings connSettings, IMessageReceiver<T> receiver, IEventBus eventPublisher, IMessagingFactory messagingFactory)
        {
            return new Listener<T>(settings, connSettings, receiver, eventPublisher, messagingFactory);
        }
    }
}
