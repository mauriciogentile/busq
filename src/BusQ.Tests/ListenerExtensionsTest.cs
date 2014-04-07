using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Threading;

namespace Ringo.BusQ.Tests
{
    /// <summary>
    ///This is a test class for ListenerExtensionsTest and is intended
    ///to contain all ListenerExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ListenerExtensionsTest : TestBase
    {
        ListenerSettings _listenerSettings;
        ConnectionSettings _connSettings;
        IMessageReceiver<Order> _receiver;

        [TestInitialize]
        public void Setup()
        {
            _listenerSettings = CreateDefaultSettings();
            _connSettings = CreateDefaultConnection();
            _receiver = CreateLocalReceiver(() => new Order());
        }

        [TestMethod]
        public void ListenerExtensionsTest_Subscribe_StatusChangedEvent()
        {
            bool onNextCalled = false;

            new Listener<Order>()
                .Set(_connSettings)
                .Set(_listenerSettings)
                .Set(_receiver)
                .OnStatusChanged(x => { onNextCalled = true; })
                .Start();

            Thread.Sleep(5000);

            Assert.AreEqual(true, onNextCalled);
        }

        [TestMethod]
        public void ListenerExtensionsTest_Subscribe_MessageReceivedEvent()
        {
            bool onNextCalled = false;

            var listener = new Listener<Order>();

            listener
                .Set(_connSettings)
                .Set(_listenerSettings)
                .Set(_receiver)
                .Subscribe(x => { onNextCalled = true; });
            
            listener.Start();

            Thread.Sleep(5000);

            Assert.AreEqual(true, onNextCalled);
        }

        [TestMethod]
        public void ListenerExtensionsTest_Subscribe_ReceptionErrorEvent()
        {
            var moq = new Mock<IMessageReceiver<Order>>();
            moq.Setup(x => x.Receive()).Throws<ApplicationException>();
            var receiver = moq.Object;

            bool onNextCalled = false;

            var listener = new Listener<Order>();

            listener
                .Set(_connSettings)
                .Set(_listenerSettings)
                .Set(receiver)
                .OnError(x => { onNextCalled = true; });

            listener.Start();

            Thread.Sleep(5000);

            Assert.AreEqual(true, onNextCalled);
        }
    }
}
