using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ
{
    public class Disposable : IDisposable
    {
        bool disposed;

        protected Action OnDispose;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && OnDispose != null)
            {
                OnDispose();
            }

            disposed = true;
        }

        ~Disposable()
        {
            Dispose(false);
        }
    }
}
