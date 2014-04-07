using System;

namespace Ringo.BusQ.Events
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
