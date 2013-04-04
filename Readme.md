#What's BusQ?

BusQ is a .NET component that makes easier for developers to work with Azure Service Bus Queues. You'll no longer have to create Topic or Queue receivers to listen for messages. It's developed in C# with Reactive Extensions.

##How BusQ works?

###Listening to a Service Bus Queue

```js
//Create the listener
var listener = new Listener();

//Configure the listener and start it
listener.From("<your-issuer-name>", "<your-secret-key>", "<namespace>")
	.Where(queueName: "MyQueue")
	.Subscribe<StatusChangedEvent>(x => { /*do something here*/ })
	.Subscribe<MessageReceivedEvent>(x => { /*and here*/ })
	.Subscribe<ReceptionErrorEvent>(x => { /*and here*/ })
	.Start();

Console.WriteLine("Listener running...");
Console.WriteLine("Preass any key to stop it");
Console.Read();

//Stop and Dispose it
listener.Stop();
listener.Dispose();
```

###Listening to a Service Bus Topic

```js
//Create the listener
using(var listener = new Listener())
{
	//Configure the listener and start it
	listener.From("<your-issuer-name>", "<your-secret-key>", "<namespace>")
		.Where(topicName: "MyTopic", subscriptionName: "MySub")
		.Subscribe<StatusChangedEvent>(x => { /*do something here*/ })
		.Subscribe<MessageReceivedEvent>(x => { /*and here*/ })
		.Subscribe<ReceptionErrorEvent>(x => { /*and here*/ })
		.Start();

	Console.WriteLine("Listener running...");
	Console.WriteLine("Preass any key to stop it");
	Console.Read();

	//Stop and Dispose it
	listener.Stop();
}
```

###An Observable Listener
```js
//Create the listener
var listener = new Listener();

//Configure the listener and start it
var listener = listener.From("<your-issuer-name>", "<your-secret-key>", "<namespace>")
				.Where(queueName: "MyQueue");

listener.AsObservable<MessageReceivedEvent>()
	.Where(x => x.Message.Label.Contains("something"))
	.Subscribe(x => { //do something here })

listener.Start();

Console.WriteLine("Listener running...");
Console.WriteLine("Preass any key to stop it");
Console.Read();

//Stop and Dispose it
listener.Stop();
listener.Dispose();
```

#Have fun!