using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ringo.BusQ.ServiceBus.Messaging;
using Ringo.BusQ.ServiceBus;
using Ringo.BusQ.ServiceBus.Messaging.Events;
using Microsoft.ServiceBus.Messaging;

namespace BusQ.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Sample2();

            Sample1();
        }

        private static void Sample2()
        {
            //Create the listener
            var listener = new Listener();

            //Configure the listener
            listener.From("<your-issuer-name>", "<your-secret-key>", "<namespace>")
                .Where(queueName: "MyQueue")
                .Subscribe<StatusChangedEvent>(x => { Console.WriteLine("Listener status changed to " + x.NewStatus); })
                .Subscribe<MessageReceivedEvent>(x => { Console.WriteLine("A message arrived at " + x.Timestamp); })
                .Subscribe<ReceptionErrorEvent>(x => { Console.WriteLine("An error has been handled here?" + x.Error); })
                .Start();

            Console.WriteLine("Listener running...");
            Console.WriteLine("Preass any key to stop it");
            Console.Read();

            listener.Stop();
            listener.Dispose();
        }

        private static void Sample1()
        {
            //Setup the listener
            //var listenerSettings = new ListenerSettings()
            //{
            //    //This case a topic with a subscription
            //    QueueOrTopicPath = "DataCollectionTopic",
            //    ReceiveMode = ReceiveMode.ReceiveAndDelete,
            //    SubscriptionName = "SystemManagerSub"
            //};

            ////Setup the connection to Azure
            //var connSettings = new ConnectionSettings()
            //{
            //    IssuerName = "owner",
            //    IssuerSecretKey = "33131654987=",
            //    ServiceBusNamespace = "CloudTestHost",
            //    ServicePath = null,
            //    ServiceUriSchema = "sb"
            //};

            ////Create an event publisher to consume events base on Reactive Extensions
            //var eventPublisher = new EventPublisher();

            //eventPublisher.GetEvent<MessageReceivedEvent>()
            //    .Subscribe<MessageReceivedEvent>(x =>
            //    {
            //        Console.WriteLine(x.Timestamp.ToString());
            //        Console.WriteLine(x.Message.ToString());
            //    });

            ////Create the listener
            //Listener listener = ListenerFactory.Create(listenerSettings, connSettings, eventPublisher);
            //listener.Start();

            //Console.ReadLine();

            //Stop it when you are done
            //listener.Stop();
        }
    }
}
