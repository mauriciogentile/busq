using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ringo.BusQ.ServiceBus.Messaging;
using Ringo.BusQ.ServiceBus;
using Ringo.BusQ.ServiceBus.Messaging.Events;
using Microsoft.ServiceBus.Messaging;
using System.Reactive.Linq;

namespace BusQ.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create the listener
            var listener = new Listener<Order>();

            //Configure the listener
            listener
                .Set("<your-issuer-name>", "<your-secret-key>", "<namespace>")
                .Set(queueName: "MyQueue")
                .OnStatusChanged(x => Console.WriteLine("Listener status changed to " + x.NewStatus))
                .OnError(x => Console.WriteLine("An error here! " + x.Error))
                .Where(x => x.CreatedDate > DateTime.Now.AddDays(-1))
                .Subscribe();

            listener.Start();

            Console.WriteLine("Listener running...");
            Console.WriteLine("Preass any key to stop it");
            Console.Read();

            listener.Stop();
            listener.Dispose();
        }
    }
}
