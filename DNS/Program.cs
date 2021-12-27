//2021
//DNS
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ObjectForDNSserver;
using System.Globalization;

namespace DNS
{
    class Program
    {
        public static List<DNSserverObject> EndPointList = new List<DNSserverObject>();
        public static int CountOfServers = 0;
        public static IPEndPoint serverip = new IPEndPoint(IPAddress.Parse("192.168.1.52"), 9876);

        static void Main(string[] args)
        {
            #region ConsoleTheme
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetWindowSize(100, 20);
            Console.Clear();
            #endregion

            TcpListener DNSserver = new TcpListener(serverip);
            DNSserver.Start();
            Console.WriteLine($"({DateTime.Now}) Listening to servers");
            Console.Title = $"DNS {serverip}";
            Thread UDPserverlistUpdate = new Thread(new ParameterizedThreadStart(UDPServerUpdateHandler));
            UDPserverlistUpdate.Start(serverip);

            while (true)
            {
                TcpClient client = DNSserver.AcceptTcpClient();

                Thread thread = new Thread(new ParameterizedThreadStart(Receive));
                thread.Start(client);
            }
        }

        static void Receive(Object inputobj)
        {
            TcpClient serverclient = (TcpClient)inputobj;
            Byte[] bytes = new Byte[256];

            NetworkStream stream = serverclient.GetStream();
            stream.Read(bytes, 0, bytes.Length);
            if (Encoding.ASCII.GetString(bytes).Contains("GET"))
            {
                Broadcast(serverclient, stream);
            }
            else
            {
                CountOfServers++;
                Console.Clear();
                Console.WriteLine($"({DateTime.Now}) Currently {CountOfServers} Servers");
                EndPointList.Add(new DNSserverObject(((IPEndPoint)serverclient.Client.RemoteEndPoint).Address.ToString(), ((IPEndPoint)serverclient.Client.RemoteEndPoint).Port));
                Console.Title = $"DNS {serverip} || Servers: {CountOfServers}";
            }
        }

        static void Broadcast(TcpClient serverclient, NetworkStream stream)
        {
            Console.WriteLine($"({DateTime.Now}) Sending List To {(IPEndPoint)serverclient.Client.RemoteEndPoint}");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, EndPointList);
        }

        static void UDPServerUpdateHandler(Object inputobj)
        {
            UdpClient UDPListener = new UdpClient();
            UDPListener.Client.Bind((IPEndPoint)inputobj);
            IPEndPoint from = new IPEndPoint(0, 0);
            while (true)
            {
                byte[] ReceivedText = UDPListener.Receive(ref from);
                if (Encoding.ASCII.GetString(ReceivedText).Contains("LISTUPDATE"))
                {
                    IPEndPoint Ipaddress = CreateIPEndPoint(Encoding.ASCII.GetString(ReceivedText).Remove(0, 11));
                    DNSserverObject serverobject = null;
                    foreach (DNSserverObject objects in EndPointList)
                    {
                        if(Ipaddress.Address.ToString() == objects.ipaddress)
                        {
                            if (Ipaddress.Port == objects.port)
                            {
                                serverobject = objects;
                            }
                        }
                    }
                    EndPointList.Remove(serverobject);
                    CountOfServers--;
                    Console.WriteLine($"({DateTime.Now}) Updating Server List");
                    Console.Title = $"DNS {serverip} || Servers: {CountOfServers}";
                }
            }
        }

        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }

        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}