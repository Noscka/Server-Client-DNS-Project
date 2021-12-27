//2021
//Client
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client_dos
{
    class Program
    {
        static void Main(string[] args)
        {
            int timeout = 0;
            TcpClient client = new TcpClient();
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

            while(true)
            {
                try
                {
                    Console.WriteLine($"Attempting Connection ({timeout}/5)");
                    client.Connect("127.0.0.1", port);
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                    if (timeout == 5)
                    {
                        Console.Clear();
                        Console.WriteLine("Connections could not be made");
                        Console.WriteLine("Aborting");
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                    }
                    timeout ++;
                }
            }

            NetworkStream datastream = client.GetStream();
            handshake(client, datastream);

            Thread t = new Thread(new ParameterizedThreadStart(HandleChat));
            t.Start(client);

            HandleInput(datastream, client, port);
        }

        static void handshake(TcpClient client, NetworkStream stream)
        {
            try
            {
                Console.Clear();
                EndPoint ReachPoint = client.Client.RemoteEndPoint;
                EndPoint LocalPoint = client.Client.LocalEndPoint;
                Console.WriteLine($"from : ({ReachPoint}) , to : ({LocalPoint})");
                Byte[] Handshake = Encoding.ASCII.GetBytes("SYN");
                Byte[] Receivingshake = new Byte[256];
                String responsestring = String.Empty;

                stream.Write(Handshake, 0, Handshake.Length);

                Int32 responsebytes = stream.Read(Receivingshake, 0, Receivingshake.Length);
                responsestring = Encoding.ASCII.GetString(Receivingshake, 0, responsebytes);

                if (responsestring == "ACK")
                {
                    Handshake = Encoding.ASCII.GetBytes("SYN-ACK");
                    stream.Write(Handshake, 0, Handshake.Length);

                    Console.WriteLine("Made Stable Connection");
                }
                else
                {
                    Console.WriteLine("Error, Unexpected handshake message");
                    stream.Close();
                    client.Close();
                    Console.WriteLine("Closing");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }


        static void Send(NetworkStream stream, string command)
        {
            try
            {
                if (command == "")
                {
                    command = "nothing";
                }

                Byte[] commandtosend = Encoding.ASCII.GetBytes(command);

                stream.Write(commandtosend, 0, commandtosend.Length);
            }
            catch (ObjectDisposedException)
            {

            }
            
        }

        static void HandleInput(NetworkStream datastream, TcpClient client, int port)
        {
            while (true)
            {
                Thread.Sleep(100);
                Console.Write(">>>");
                string input = Console.ReadLine();
                input = input.ToLower();
                Send(datastream, input);
                switch (input)
                {
                    case "exit":
                        datastream.Close();
                        client.Close();
                        Console.WriteLine("Closing");
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                        break;

                    case "disconnect":
                        datastream.Close();
                        client.Close();
                        Console.WriteLine("Disconnecting");
                        break;

                    case "connect":
                        client.Connect("127.0.0.1", port);
                        datastream = client.GetStream();
                        handshake(client, datastream);
                        break;
                }
            }
        }

        static void HandleChat(object InputObj)
        {
            TcpClient client = (TcpClient)InputObj;
            NetworkStream stream = client.GetStream();

            Byte[] Response = new byte[256];
            String ChatString = String.Empty;


            while (true)
            {
                Int32 ChatBytes = stream.Read(Response, 0, Response.Length);
                ChatString = Encoding.ASCII.GetString(Response, 0, ChatBytes);
                if (ChatString.Contains(";chat message;"))
                {
                    ChatString = ChatString.Replace(";chat message;", String.Empty);
                    Console.WriteLine(Environment.NewLine + $"Received Message: {ChatString}");
                }
                else
                {
                    Console.WriteLine(ChatString);
                }
            }
        }
    }
}
