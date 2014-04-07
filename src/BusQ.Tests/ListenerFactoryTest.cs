using NUnit.Framework;
using Ringo.BusQ.Events;
using Moq;

namespace Ringo.BusQ.Tests
{
    [TestFixture]
    public class ListenerFactoryTest : TestBase
    {
        const string QueueName = "queueName";
        const string IssuerName = "issuerName";
        const string IssuerKey = "issuerKey";
        const string ServiceBusNamespace = "serviceBusNamespace";
        private const string TopicName = "topicName";
        private const string SubscriptionName = "subscriptionName";

        ListenerSettings _settings;
        ConnectionSettings _connSettings;
        IEventBus _eventPublisher;

        [SetUp]
        public void Setup()
        {
            _settings = CreateDefaultSettings();
            _connSettings = CreateDefaultConnection();
            _eventPublisher = new DefaultEventBus();
        }

        [Test]
        public void ListenerFactory_Create_Test()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(QueueName, IssuerName, IssuerKey, ServiceBusNamespace, 
                _eventPublisher);

            Assert.AreEqual(QueueName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(IssuerName, target.ConnectionSettings.IssuerName);
            Assert.AreEqual(IssuerKey, target.ConnectionSettings.IssuerSecretKey);
            Assert.AreEqual(ServiceBusNamespace, target.ConnectionSettings.ServiceBusNamespace);
            Assert.AreEqual(_eventPublisher, target.EventBus);
        }

        [Test]
        public void ListenerFactory_Create_Test1()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_settings, _connSettings, _eventPublisher);

            Assert.AreEqual(_settings, target.ListenerSettings);
            Assert.AreEqual(_connSettings, target.ConnectionSettings);
            Assert.AreEqual(_eventPublisher, target.EventBus);
        }

        [Test]
        public void ListenerFactory_Create_Test2()
        {
            var receiver = new Mock<IMessageReceiver<Order>>().Object;

            Listener<Order> target = ListenerFactory<Order>.Create(_settings, _connSettings, receiver, _eventPublisher);

            Assert.AreEqual(_settings, target.ListenerSettings);
            Assert.AreEqual(_connSettings, target.ConnectionSettings);
            Assert.AreEqual(_eventPublisher, target.EventBus);
            Assert.AreEqual(receiver, target.MessageReceiver);
        }

        [Test]
        public void ListenerFactory_Create_Test3()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(TopicName, SubscriptionName, IssuerName, IssuerKey, ServiceBusNamespace, _eventPublisher);

            Assert.AreEqual(TopicName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(SubscriptionName, target.ListenerSettings.SubscriptionName);
            Assert.AreEqual(IssuerName, target.ConnectionSettings.IssuerName);
            Assert.AreEqual(IssuerKey, target.ConnectionSettings.IssuerSecretKey);
            Assert.AreEqual(ServiceBusNamespace, target.ConnectionSettings.ServiceBusNamespace);
            Assert.AreEqual(_eventPublisher, target.EventBus);
        }

        [Test]
        public void ListenerFactory_Create_Test4()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(QueueName, _connSettings, _eventPublisher);

            Assert.AreEqual(QueueName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(_connSettings, target.ConnectionSettings);
            Assert.AreEqual(_eventPublisher, target.EventBus);
        }

        [Test]
        public void ListenerFactory_Create_Test5()
        {
            var receiver = new Mock<IMessageReceiver<Order>>().Object;

            Listener<Order> target = ListenerFactory<Order>.Create(QueueName, _connSettings, receiver, _eventPublisher);

            Assert.AreEqual(QueueName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(_connSettings, target.ConnectionSettings);
            Assert.AreEqual(_eventPublisher, target.EventBus);
            Assert.AreEqual(receiver, target.MessageReceiver);
        }

        [Test]
        public void ListenerFactory_Create_Test6()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(TopicName, SubscriptionName, _connSettings, 
                _eventPublisher);

            Assert.AreEqual(TopicName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(SubscriptionName, target.ListenerSettings.SubscriptionName);
            Assert.AreEqual(_connSettings, target.ConnectionSettings);
            Assert.AreEqual(_eventPublisher, target.EventBus);
        }


        [Test]
        public void ListenerFactory_Create_Test7()
        {
            var receiver = new Mock<IMessageReceiver<Order>>().Object;
            IMessagingFactory factory = new Mock<IMessagingFactory>().Object;

            Listener<Order> target = ListenerFactory<Order>.Create(_settings, _connSettings, receiver, 
                _eventPublisher, factory);

            Assert.AreEqual(_settings, target.ListenerSettings);
            Assert.AreEqual(_connSettings, target.ConnectionSettings);
            Assert.AreEqual(_eventPublisher, target.EventBus);
            Assert.AreEqual(receiver, target.MessageReceiver);
            Assert.AreEqual(factory, target.MessagingFactory);
        }
    }
}
