using System;
using System.Net;

namespace ObjectForDNSserver
{
    [Serializable]
    public class DNSserverObject
    {
        public string ipaddress { get; set; }
        public int port { get; set; }
        public DNSserverObject(string Ipaddress, int Port)
        {
            ipaddress = Ipaddress;
            port = Port;
        }
    }
}