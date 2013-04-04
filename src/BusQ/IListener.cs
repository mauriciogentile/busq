using System;
namespace Ringo.BusQ.ServiceBus.Messaging
{
    public interface IListener
    {
        ListenerStatus Status { get; }
        void Resume();
        void Start();
        void Pause();
        void Stop();
    }
}
