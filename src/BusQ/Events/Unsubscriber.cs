using System;
using System.Collections.Concurrent;
using Ringo.BusQ.Util;

namespace Ringo.BusQ.Events
{
    public class Unsubscriber<T> : Disposable
    {
        private readonly ConcurrentDictionary<Type, object> _subjects;
        private readonly Type _subject;

        public Unsubscriber(ConcurrentDictionary<Type, object> subjects, Type subject)
        {
            _subjects = subjects;
            _subject = subject;
            OnDispose = () =>
            {
                if (_subject != null && _subjects.ContainsKey(_subject))
                {
                    object obj;
                    _subjects.TryRemove(_subject, out obj);
                }
            };
        }
    }
}
