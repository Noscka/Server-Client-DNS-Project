//2021
//Server
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TcpClient> ChatAbleUsers = new List<TcpClient>();
            Int32 port;
            while (true)
            {
                Console.Write("Enter a port: ");
                string inputport = Console.ReadLine();
                try
                {
                    port = Int32.Parse(inputport);
                    Console.Clear();
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please input a corrent port");
                }
            }

            IPAddress localAddr = IPAddress.Parse("192.168.1.80");
            IPEndPoint serverip = new IPEndPoint(IPAddress.Any, port);
            TcpListener server = new TcpListener(serverip);


            //DNS
            byte[] DNS_TCPconnectionMessage = Encoding.ASCII.GetBytes("SERVER");
            TcpClient DNSsender = new TcpClient();
            DNSsender.Client.Bind(serverip);
            DNSsender.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.52"), 9876));
            NetworkStream stream = DNSsender.GetStream();
            stream.Write(DNS_TCPconnectionMessage, 0, DNS_TCPconnectionMessage.Length);
            stream.Close();
            DNSsender.Close();

            Thread UDPthread = new Thread(new ParameterizedThreadStart(UDPHander));
            UDPthread.Start(serverip);
            //DNS


            server.Start();
            Byte[] bytes = new Byte[256];
            int usercount = 0;

            Console.WriteLine($"Starting server on: {server.LocalEndpoint}");
            while (true)
            {
                Console.WriteLine("Waiting for a connection... ");
                TcpClient client = server.AcceptTcpClient();

                object PassingArguments = new object[3] {client, usercount, ChatAbleUsers};

                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(PassingArguments);
                usercount++;
            }

        }

        static void HandleClient(Object obj)
        {
            Array ArgumentArray = (Array)obj;
            TcpClient client = (TcpClient)ArgumentArray.GetValue(0);
            int userID = (int)ArgumentArray.GetValue(1);
            List<TcpClient> ChatAbleUsers = (List<TcpClient>)ArgumentArray.GetValue(2);
            NetworkStream stream = client.GetStream();
            Byte[] bytes = new Byte[256];


            Console.WriteLine("Connection Made");
            String ReceivedData = String.Empty;

            HandShake.HandShakeLoop(client, stream, userID, ChatAbleUsers);

            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                ReceivedData = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine($"User {userID}>>> Received : '{ReceivedData}'");
                Byte[] SendingData = null;
                Byte[] ChatSend = null;
                if (ReceivedData.ToLower().Contains("echo "))
                {
                    string echoreponse = ReceivedData.Remove(0, 5);
                    if (echoreponse == "")
                    {
                        SendingData = Encoding.ASCII.GetBytes("You need to type a word to say");
                    }
                    else
                    {
                        SendingData = Encoding.ASCII.GetBytes(echoreponse);
                    }
                }
                else if (ReceivedData.ToLower().Contains("chat "))
                {
                    string ChatMessage = ReceivedData.Remove(0, 5);
                    if (ChatMessage == "")
                    {
                        SendingData = Encoding.ASCII.GetBytes("You need to type a word to say");
                    }
                    else
                    {
                        SendingData = Encoding.ASCII.GetBytes($"Sent Message to all clients : {ChatMessage}");
                        ChatSend = Encoding.ASCII.GetBytes(ChatMessage.Insert(0, ";chat message;"));

                        foreach (TcpClient ServerConnectedClients in ChatAbleUsers)
                        {
                            if (!(ServerConnectedClients == client))
                            {
                                NetworkStream temStream = ServerConnectedClients.GetStream();
                                temStream.Write(ChatSend, 0, ChatSend.Length);
                            }
                        }
                    }
                }
                else if (ReceivedData.ToLower() == "nothing")
                {
                    SendingData = Encoding.ASCII.GetBytes("Please Type Something");
                }
                else if (ReceivedData == "FIN")
                {
                    ChatAbleUsers.Remove(client);
                    userID--;
                    stream.Close();
                    client.Close();
                    return;
                }
                else if (ReceivedData.ToLower() == "help")
                {
                    SendingData = Encoding.ASCII.GetBytes(@"help - this command
echo - says what you tell it to say (echo {words}) - wip, makes everything lowercase
chat - send chat to all clients (chat {words}");
                }
                else
                {
                    SendingData = Encoding.ASCII.GetBytes("Error : not a real command, check spelling");
                }
                stream.Write(SendingData, 0, SendingData.Length);
                Console.WriteLine($"User {userID}>>> Responded : '{Encoding.ASCII.GetString(SendingData)}'");
            }

            client.Close();
        }

        static void UDPHander(object inputobj)
        {
            UdpClient clientListener = new UdpClient();
            clientListener.Client.Bind((IPEndPoint)inputobj);
            clientListener.Client.ReceiveTimeout = 0;
            while (true)
            {
                IPEndPoint from = new IPEndPoint(0, 0);
                byte[] Request = clientListener.Receive(ref from);
                Console.WriteLine($"User {from} is doing servercheck");
                byte[] Response = null;

                if (Encoding.ASCII.GetString(Request).Contains("SERVERCHECK"))
                {
                    Response = Encoding.ASCII.GetBytes("ACK-SERVERCHECK");
                    clientListener.Send(Response, Response.Length, from);
                }
                else
                {
                    Response = Encoding.ASCII.GetBytes($"SERVER-{(IPEndPoint)inputobj}");
                    clientListener.Send(Response, Response.Length, from);
                }
            }
        }
    }

    public class HandShake
    {
        public static void HandShakeLoop(TcpClient client,NetworkStream stream, int userID, List<TcpClient> ChatAbleUsers)
        {
            Byte[] bytes = new Byte[256];
            String ReceivedData = String.Empty;
            Int32 ResponseLength = 0;


            while ((ResponseLength = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                ReceivedData = Encoding.ASCII.GetString(bytes, 0, ResponseLength);
                byte[] SendingData = null;
                switch (ReceivedData)
                {
                    case "SYN":
                        SendingData = Encoding.ASCII.GetBytes("ACK");
                        break;
                    case "SYN-ACK-GUI":
                        ChatAbleUsers.Add(client); 
                        Console.WriteLine($"<<<GUI User {userID}>>> Made Stable Connection");
                        return;
                    case "SYN-ACK":
                        Console.WriteLine($"<<<User {userID}>>> Made Stable Connection");
                        return;
                }

                stream.Write(SendingData, 0, SendingData.Length);
            }
        }
    }
}
