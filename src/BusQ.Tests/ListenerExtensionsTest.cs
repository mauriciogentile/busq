using Ringo.BusQ.ServiceBus.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ringo.BusQ.ServiceBus.Messaging.Events;
using Ringo.BusQ.ServiceBus;
using System.Collections.ObjectModel;
using Moq;
using System.Threading;
using System.Reactive.Linq;

namespace Ringo.BusQ.Tests
{


    /// <summary>
    ///This is a test class for ListenerExtensionsTest and is intended
    ///to contain all ListenerExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ListenerExtensionsTest : TestBase
    {
        [TestMethod]
        public void ListenerExtensionsTest_Subscribe_StatusChangedEvent()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();

            bool onNextCalled = false;

            new Listener<Order>()
                .Set(connSettings)
                .Set(listenerSettings)
                .Set(receiver)
                .OnStatusChanged(x => { onNextCalled = true; })
                .Start();

            Thread.Sleep(5000);

            Assert.AreEqual(true, onNextCalled);
        }

        [TestMethod]
        public void ListenerExtensionsTest_Subscribe_MessageReceivedEvent()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();
            IMessageReceiver receiver = CreateLocalReceiver();

            bool onNextCalled = false;

            var listener = new Listener<Order>();

            listener
                .Set(connSettings)
                .Set(listenerSettings)
                .Set(receiver)
                .Subscribe(x => { onNextCalled = true; });
            
            listener.Start();

            Thread.Sleep(5000);

            Assert.AreEqual(true, onNextCalled);
        }

        [TestMethod]
        public void ListenerExtensionsTest_Subscribe_ReceptionErrorEvent()
        {
            ListenerSettings listenerSettings = CreateDefaultSettings();
            ConnectionSettings connSettings = CreateDefaultConnection();

            var moq = new Mock<IMessageReceiver>();
            moq.Setup(x => x.Receive()).Throws<ApplicationException>();
            IMessageReceiver receiver = moq.Object;

            bool onNextCalled = false;

            var listener = new Listener<Order>();

            listener
                .Set(connSettings)
                .Set(listenerSettings)
                .Set(receiver)
                .OnError(x => { onNextCalled = true; });

            listener.Start();

            Thread.Sleep(5000);

            Assert.AreEqual(true, onNextCalled);
        }
    }
}
