using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus.Messaging.Events
{
    public interface IEventBus : IPublisher, ISubscriber
    {
    }

    public interface IPublisher
    {
        void Publish<TEvent>(TEvent sampleEvent);
    }

    public interface ISubscriber
    {
        Unsubscriber<TEvent> Subscribe<TEvent>(Action<TEvent> handler);
    }
}
