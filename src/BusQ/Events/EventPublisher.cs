using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Ringo.BusQ.ServiceBus.Messaging.Events
{
    public class EventPublisher : IEventBus
    {
        private readonly ConcurrentDictionary<Type, object> subjects = new ConcurrentDictionary<Type, object>();

        public void Publish<TEvent>(TEvent eventData)
        {
            object subject;
            if (subjects.TryGetValue(typeof(TEvent), out subject))
            {
                var observer = (ISubject<TEvent>)subject;
                observer.OnNext(eventData);
            }
        }

        public Unsubscriber<TEvent> Subscribe<TEvent>(Action<TEvent> handler)
        {
            var subject = (ISubject<TEvent>)subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());
            subject.AsObservable().Subscribe(handler);
            return new Unsubscriber<TEvent>(subjects, typeof(TEvent));
        }
    }
}
