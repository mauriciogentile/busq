using System;

namespace Ringo.BusQ
{
    public interface IMessageReceiver<out T>
    {
        T Receive();
        T Receive(TimeSpan pollingTime);
        T Receive(long longSeuence);
        bool IsClosed { get; }
        void Close();
    }
}
