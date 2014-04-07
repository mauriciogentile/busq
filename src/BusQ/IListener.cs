namespace Ringo.BusQ
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
