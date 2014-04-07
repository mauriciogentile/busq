using Microsoft.ServiceBus.Messaging;

namespace Ringo.BusQ
{
    public interface IMessagingFactory
    {
        IMessageReceiver<T> CreateMessageReceiver<T>(string entityPath, ReceiveMode receiveMode);
        bool IsClosed { get; }
        void Close();
    }
}
