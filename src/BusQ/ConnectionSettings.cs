namespace Ringo.BusQ
{
    public class ConnectionSettings
    {
        public string IssuerName { get; set; }
        public string IssuerSecretKey { get; set; }
        public string ServiceUriSchema { get; set; }
        public string ServiceBusNamespace { get; set; }
        public string ServicePath { get; set; }
    }
}
