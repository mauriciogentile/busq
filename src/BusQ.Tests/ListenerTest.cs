using Ringo.BusQ;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using Ringo.BusQ.ServiceBus.Messaging;
using Ringo.BusQ.ServiceBus;
using Moq;
using System.Diagnostics;
using Ringo.BusQ.ServiceBus.Messaging.Events;

namespace Ringo.BusQ.Tests
{
    [TestClass()]
    public class ListenerTest : TestBase
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Listener_Should_Pause()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);
            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Pause();
        }

        [TestMethod()]
        public void Listener_Should_ListenerResume()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);
            target.Resume();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
        }

        [TestMethod()]
        public void Listener_Should_Start_And_Stop()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);
        }

        [TestMethod()]
        public void Listener_Should_Start_And_Stop_And_Call_EventAgregator_Publish_StatusChangedEvent()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            var moq = new Mock<IEventPublisher>();
            moq.Setup(x => x.Publish(It.IsAny<StatusChangedEvent>()));
            IEventPublisher eventPublisher = moq.Object;

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);

            moq.Verify(x => x.Publish(It.IsAny<StatusChangedEvent>()), Times.Exactly(2));
        }

        [TestMethod()]
        public void Listener_Should_Start_And_Stop_And_Call_EventAgregator_Publish_MessageReceivedEvent()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            var moq = new Mock<IEventPublisher>();
            moq.Setup(x => x.Publish(It.IsAny<MessageReceivedEvent>()));
            IEventPublisher eventPublisher = moq.Object;

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);

            moq.Verify(x => x.Publish(It.IsAny<MessageReceivedEvent>()), Times.AtLeastOnce());
        }

        [TestMethod()]
        public void Listener_Should_Start_And_Stop_And_Call_EventAgregator_Publish_ReceptionErrorEvent()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();

            var moq = new Mock<IMessageReceiver>();
            moq.Setup(x => x.Receive()).Throws<ApplicationException>();
            IMessageReceiver receiver = moq.Object;

            var moq2 = new Mock<IEventPublisher>();
            moq2.Setup(x => x.Publish(It.IsAny<MessageReceivedEvent>()));
            IEventPublisher eventPublisher = moq2.Object;

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);

            moq2.Verify(x => x.Publish(It.IsAny<ReceptionErrorEvent>()), Times.AtLeastOnce());
        }

        [TestMethod()]
        public void Listener_Should_Start_And_Stop_And_Call_MessagingFactory_CreateMessageReceiver()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalClosedReceiver();
            IMessageReceiver receiver2 = CreateLocalReceiver();

            var moq1 = new Mock<IMessagingFactory>();
            moq1.Setup(x => x.CreateMessageReceiver(It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(receiver2);
            IMessagingFactory messagingFactory = moq1.Object;

            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher, messagingFactory);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);

            moq1.Verify(x => x.CreateMessageReceiver(It.IsAny<string>(), It.IsAny<ReceiveMode>()), Times.Once());
        }

        [TestMethod()]
        public void Listener_Should_Start_And_Pause()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Pause();
            Assert.AreEqual(ListenerStatus.Paused, target.Status);
        }

        [TestMethod()]
        public void Listener_Should_Start_Pause_And_Resume()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Pause();
            Assert.AreEqual(ListenerStatus.Paused, target.Status);
            target.Resume();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
        }

        [TestMethod()]
        public void Listener_Should_Start_Pause_Resume_And_Stop()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.ListeningDefaultMilliseconds);
            target.Pause();
            Assert.AreEqual(ListenerStatus.Paused, target.Status);
            target.Resume();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Listener_Should_Stop()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();
            IEventPublisher eventPublisher = new EventPublisher();

            Listener target = ListenerFactory.Create(listenerSettings, connSettings, receiver, eventPublisher);
            target.Stop();
        }
    }
}
