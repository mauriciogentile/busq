#What's BusQ?

BusQ is a .NET component that makes easier for developers to work with Azure Service Bus Queues.
You'll no longer have to create Topic or Queue receivers to listen for messages.
It's developed in C# with Reactive Extensions.

##How BusQ works?

###Listening to a Service Bus Queue

```js
//Create the listener
var listener = new Listener<Order>();

//Configure the listener
listener
    .Set("<your-issuer-name>", "<your-secret-key>", "<namespace>")
    .Set(queueName: "MyQueue")
    .OnStatusChanged(x => Console.WriteLine("Listener status changed to " + x.NewStatus))
    .OnError(x => Console.WriteLine("An error here! " + x.Error))
    .Where(x => x.CreatedDate > DateTime.Now.AddDays(-1)) //Subscribe to order since yesterday only
    .Subscribe();

listener.Start();

Console.WriteLine("Listener running...");
Console.WriteLine("Preass any key to stop it");
Console.Read();

listener.Stop();
listener.Dispose();
```

##Have fun!