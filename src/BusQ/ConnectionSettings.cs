using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus
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
