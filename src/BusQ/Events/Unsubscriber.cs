using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Ringo.BusQ
{
    public class Unsubscriber<T> : Disposable
    {
        private ConcurrentDictionary<Type, object> _subjects;
        private Type _subject;

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
