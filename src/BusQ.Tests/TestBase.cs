using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ringo.BusQ.ServiceBus;
using Ringo.BusQ.ServiceBus.Messaging;
using Microsoft.ServiceBus.Messaging;
using Moq;
using System.Threading;

namespace Ringo.BusQ.Tests
{
    [TestClass()]
    public class TestBase
    {
        public TestContext TestContext { get; set; }

        public static ListenerSettings CreateDefaultSettings()
        {
            ListenerSettings listenerSettings = new ListenerSettings()
            {
                QueueOrTopicPath = "DataCollectionTopic",
                ReceiveMode = ReceiveMode.ReceiveAndDelete,
                SubscriptionName = "SystemManagerSub"
            };

            return listenerSettings;
        }

        public static ConnectionSettings CreateDefaultConnection()
        {
            ConnectionSettings connSettings = new ConnectionSettings()
            {
                IssuerName = "owner",
                IssuerSecretKey = "CAMUAEsCALZvkKxnlTpSDgijOzNJ78as5B0ektMHpGs=",
                ServiceBusNamespace = "CloudTestHost",
                ServicePath = null,
                ServiceUriSchema = "sb"
            };

            return connSettings;
        }

        public static IMessageReceiver CreateLocalReceiver()
        {
            BrokeredMessage message = new BrokeredMessage(Guid.Empty);

            var moq = new Mock<IMessageReceiver>();
            moq.Setup(x => x.Receive()).Returns(() => { Thread.Sleep(Constants.ReceiverDelayMilliseconds); return message; });
            moq.Setup(x => x.Receive(It.IsAny<TimeSpan>())).Returns(() => { Thread.Sleep(Constants.ReceiverDelayMilliseconds); return message; });
            moq.Setup(x => x.Receive(It.IsAny<long>())).Returns(() => { Thread.Sleep(Constants.ReceiverDelayMilliseconds); return message; });

            return moq.Object;
        }

        public static IMessageReceiver CreateLocalClosedReceiver()
        {
            BrokeredMessage message = new BrokeredMessage(Guid.Empty);

            var moq = new Mock<IMessageReceiver>();
            moq.Setup(x => x.Receive()).Returns(() => { Thread.Sleep(Constants.ReceiverDelayMilliseconds); return message; });
            moq.Setup(x => x.Receive(It.IsAny<TimeSpan>())).Returns(() => { Thread.Sleep(Constants.ReceiverDelayMilliseconds); return message; });
            moq.Setup(x => x.Receive(It.IsAny<long>())).Returns(() => { Thread.Sleep(Constants.ReceiverDelayMilliseconds); return message; });
            moq.Setup(x => x.IsClosed).Returns(true);

            return moq.Object;
        }
    }
}
