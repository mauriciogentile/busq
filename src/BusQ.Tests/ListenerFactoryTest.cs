using Ringo.BusQ;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ringo.BusQ.ServiceBus.Messaging;
using Ringo.BusQ.ServiceBus;
using Ringo.BusQ.ServiceBus.Messaging.Events;
using Moq;

namespace Ringo.BusQ.Tests
{
    [TestClass()]
    public class ListenerFactoryTest : TestBase
    {
        [TestMethod()]
        public void ListenerFactory_Create_Test()
        {
            string queueName = "queueName";
            string issuerName = "issuerName";
            string issuerKey = "issuerKey";
            string serviceBusNamespace = "serviceBusNamespace";
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(queueName, issuerName, issuerKey, serviceBusNamespace, eventPublisher);

            Assert.AreEqual(queueName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(issuerName, target.ConnectionSettings.IssuerName);
            Assert.AreEqual(issuerKey, target.ConnectionSettings.IssuerSecretKey);
            Assert.AreEqual(serviceBusNamespace, target.ConnectionSettings.ServiceBusNamespace);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
        }

        [TestMethod()]
        public void ListenerFactory_Create_Test1()
        {
            ListenerSettings settings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(settings, connSettings, eventPublisher);

            Assert.AreEqual(settings, target.ListenerSettings);
            Assert.AreEqual(connSettings, target.ConnectionSettings);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
        }

        [TestMethod()]
        public void ListenerFactory_Create_Test2()
        {
            ListenerSettings settings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IEventPublisher eventPublisher = new EventPublisher();
            IMessageReceiver receiver = new Mock<IMessageReceiver>().Object;

            Listener target = ListenerFactory.Create(settings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(settings, target.ListenerSettings);
            Assert.AreEqual(connSettings, target.ConnectionSettings);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
            Assert.AreEqual(receiver, target.MessageReceiver);
        }

        [TestMethod()]
        public void ListenerFactory_Create_Test3()
        {
            string topicName = "topicName";
            string subscriptionName = "subscriptionName";
            string issuerName = "issuerName";
            string issuerKey = "issuerKey";
            string serviceBusNamespace = "serviceBusNamespace";
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(topicName, subscriptionName, issuerName, issuerKey, serviceBusNamespace, eventPublisher);

            Assert.AreEqual(topicName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(subscriptionName, target.ListenerSettings.SubscriptionName);
            Assert.AreEqual(issuerName, target.ConnectionSettings.IssuerName);
            Assert.AreEqual(issuerKey, target.ConnectionSettings.IssuerSecretKey);
            Assert.AreEqual(serviceBusNamespace, target.ConnectionSettings.ServiceBusNamespace);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
        }

        [TestMethod()]
        public void ListenerFactory_Create_Test4()
        {
            string queueName = "queueName";
            ConnectionSettings connSettings = CreateDefaultConnection();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(queueName, connSettings, eventPublisher);

            Assert.AreEqual(queueName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(connSettings, target.ConnectionSettings);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
        }

        [TestMethod()]
        public void ListenerFactory_Create_Test5()
        {
            string queueName = "queueName";
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = new Mock<IMessageReceiver>().Object;
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(queueName, connSettings, receiver, eventPublisher);

            Assert.AreEqual(queueName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(connSettings, target.ConnectionSettings);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
            Assert.AreEqual(receiver, target.MessageReceiver);
        }

        [TestMethod()]
        public void ListenerFactory_Create_Test6()
        {
            string topicName = "topicName";
            string subscriptionName = "subscriptionName";
            ConnectionSettings connSettings = CreateDefaultConnection();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(topicName, subscriptionName, connSettings, eventPublisher);

            Assert.AreEqual(topicName, target.ListenerSettings.QueueOrTopicPath);
            Assert.AreEqual(subscriptionName, target.ListenerSettings.SubscriptionName);
            Assert.AreEqual(connSettings, target.ConnectionSettings);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
        }


        [TestMethod()]
        public void ListenerFactory_Create_Test7()
        {
            ListenerSettings settings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IEventPublisher eventPublisher = new EventPublisher();
            IMessageReceiver receiver = new Mock<IMessageReceiver>().Object;
            IMessagingFactory factory = new Mock<IMessagingFactory>().Object;

            Listener target = ListenerFactory.Create(settings, connSettings, receiver, eventPublisher, factory);

            Assert.AreEqual(settings, target.ListenerSettings);
            Assert.AreEqual(connSettings, target.ConnectionSettings);
            Assert.AreEqual(eventPublisher, target.EventPublisher);
            Assert.AreEqual(receiver, target.MessageReceiver);
            Assert.AreEqual(factory, target.MessagingFactory);
        }
    }
}
