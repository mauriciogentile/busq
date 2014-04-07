using System;
using NUnit.Framework;
using Microsoft.ServiceBus.Messaging;
using Moq;
using System.Threading;

namespace Ringo.BusQ.Tests
{
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

        public static IMessageReceiver<T> CreateLocalReceiver<T>(Func<T> generator)
        {
            T message = generator();

            var moq = new Mock<IMessageReceiver<T>>();
            moq.Setup(x => x.Receive()).Returns(() => { Thread.Sleep(Constants.HalfSecond); return message; });
            moq.Setup(x => x.Receive(It.IsAny<TimeSpan>())).Returns(() => { Thread.Sleep(Constants.HalfSecond); return message; });
            moq.Setup(x => x.Receive(It.IsAny<long>())).Returns(() => { Thread.Sleep(Constants.HalfSecond); return message; });

            return moq.Object;
        }

        public static IMessageReceiver<T> CreateLocalClosedReceiver<T>(Func<T> generator)
        {
            T message = generator();

            var moq = new Mock<IMessageReceiver<T>>();
            moq.Setup(x => x.Receive()).Returns(() => { Thread.Sleep(Constants.HalfSecond); return message; });
            moq.Setup(x => x.Receive(It.IsAny<TimeSpan>())).Returns(() => { Thread.Sleep(Constants.HalfSecond); return message; });
            moq.Setup(x => x.Receive(It.IsAny<long>())).Returns(() => { Thread.Sleep(Constants.HalfSecond); return message; });
            moq.Setup(x => x.IsClosed).Returns(true);

            return moq.Object;
        }
    }
}
