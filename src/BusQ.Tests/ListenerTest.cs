using System;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using Ringo.BusQ.Events;
using Moq;
using NUnit.Framework;

namespace Ringo.BusQ.Tests
{
    [TestFixture]
    public class ListenerTest : TestBase
    {
        ListenerSettings _listenerSettings;
        ConnectionSettings _connSettings;
        IMessageReceiver<Order> _receiver;
        IEventBus _eventPublisher;

        [SetUp]
        public void Setup()
        {
            _listenerSettings = CreateDefaultSettings();
            _connSettings = CreateDefaultConnection();
            _receiver = CreateLocalReceiver(() => new Order());
            _eventPublisher = new DefaultEventBus();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Listener_Should_Pause()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Pause();
        }

        [Test]
        public void Listener_Should_ListenerResume()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);
            target.Resume();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.OneSecond);
            target.Stop();
        }

        [Test]
        public void Listener_Should_Start_And_Stop()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.OneSecond);
            target.Stop();
            Assert.AreEqual(ListenerStatus.Stopped, target.Status);
        }

        [Test]
        public void Listener_Should_Start_And_Stop_And_Call_EventAgregator_Publish_StatusChangedEvent()
        {
            var moq = new Mock<IEventBus>();
            moq.Setup(x => x.Publish(It.IsAny<StatusChangedEvent>()));
            IEventBus eventPublisher = moq.Object;

            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, eventPublisher);

            target.Start();
            
            Thread.Sleep(Constants.OneSecond);

            target.Stop();

            moq.Verify(x => x.Publish(It.IsAny<StatusChangedEvent>()), Times.Exactly(2));
        }

        [Test]
        public void Listener_Should_Start_And_Stop_And_Call_EventAgregator_Publish_MessageReceivedEvent()
        {
            var moq = new Mock<IEventBus>();
            moq.Setup(x => x.Publish(It.IsAny<Order>()));
            IEventBus eventPublisher = moq.Object;

            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, eventPublisher);

            target.Start();

            Thread.Sleep(Constants.OneSecond);
            
            target.Stop();

            moq.Verify(x => x.Publish(It.IsAny<Order>()), Times.AtLeastOnce());
        }

        [Test]
        public void Listener_Should_Start_And_Stop_And_Call_EventAgregator_Publish_ReceptionErrorEvent()
        {
            var moq = new Mock<IMessageReceiver<Order>>();
            moq.Setup(x => x.Receive()).Throws<ApplicationException>();
            IMessageReceiver<Order> receiver = moq.Object;

            var moq2 = new Mock<IEventBus>();
            moq2.Setup(x => x.Publish(It.IsAny<MessageReceivedEvent<Order>>()));
            IEventBus eventPublisher = moq2.Object;

            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                receiver, eventPublisher);

            target.Start();

            Thread.Sleep(Constants.OneSecond);
            
            target.Stop();

            moq2.Verify(x => x.Publish(It.IsAny<ReceptionErrorEvent>()), Times.AtLeastOnce());
        }

        [Test]
        public void Listener_Should_Start_And_Stop_And_Call_MessagingFactory_CreateMessageReceiver()
        {
            var moq1 = new Mock<IMessagingFactory>();
            moq1.Setup(x => x.CreateMessageReceiver<Order>(It.IsAny<string>(), It.IsAny<ReceiveMode>()))
                .Returns(_receiver);
            IMessagingFactory messagingFactory = moq1.Object;

            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings, null,
                _eventPublisher, messagingFactory);

            target.Start();
            
            Thread.Sleep(5000);
            
            target.Stop();
            
            moq1.Verify(x => x.CreateMessageReceiver<Order>(It.IsAny<string>(), It.IsAny<ReceiveMode>()), Times.Once());
        }

        [Test]
        public void Listener_Should_Start_And_Pause()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.OneSecond);
            target.Pause();
            Assert.AreEqual(ListenerStatus.Paused, target.Status);
        }

        [Test]
        public void Listener_Should_Start_Pause_And_Resume()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);
            target.Start();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
            Thread.Sleep(Constants.OneSecond);
            target.Pause();
            Assert.AreEqual(ListenerStatus.Paused, target.Status);
            target.Resume();
            Assert.AreEqual(ListenerStatus.Running, target.Status);
        }

        [Test]
        public void Listener_Should_Start_Pause_Resume_And_Stop()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);

            Assert.AreEqual(ListenerStatus.NotStarted, target.Status);

            target.Start();

            Assert.AreEqual(ListenerStatus.Running, target.Status);

            Thread.Sleep(Constants.OneSecond);

            target.Pause();

            Assert.AreEqual(ListenerStatus.Paused, target.Status);

            target.Resume();

            Assert.AreEqual(ListenerStatus.Running, target.Status);

            target.Stop();

            Assert.AreEqual(ListenerStatus.Stopped, target.Status);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Listener_Should_Stop()
        {
            Listener<Order> target = ListenerFactory<Order>.Create(_listenerSettings, _connSettings,
                _receiver, _eventPublisher);
            target.Stop();
        }
    }
}
