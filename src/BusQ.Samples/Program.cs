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
    }
}
