using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus.Messaging.Events
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent sampleEvent);
        IObservable<TEvent> GetEvent<TEvent>();
    }
}
