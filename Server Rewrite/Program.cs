//2021
//Server Rewrite
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

namespace Server_Rewrite
{
    class Program
    {
        #region Variables
        public static List<TcpClient> ClientsChat = new List<TcpClient>(); // List of users that can get sent chat messages
        public static int UserCount = 0; // count for keeping track of user count
        public static IPEndPoint ServerIPEndPoint = new IPEndPoint(0, 0); // Server IPEndPoint
        #endregion

        static void Main(string[] args)
        {
            #region temp
            string serverip = "127.0.0.1"; //Temp IP
            #endregion

            #region ConsoleTheme
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetWindowSize(100, 15);
            Console.Clear();
            #endregion

            #region IP:PORT
            //Region to turn input port and soon to be input ip to endpoint

            if (!(args.Length == 0))
            {
                ServerIPEndPoint = new IPEndPoint(IPAddress.Any, int.Parse(args[0])); // Creates end point from Local IP and arg input port
            }
            else
            {
                while (true)
                {
                    Console.Write($"Enter Port:: {serverip}:");
                    try
                    {
                        ServerIPEndPoint = new IPEndPoint(IPAddress.Any, int.Parse(Console.ReadLine())); // Turns Console input into int and uses that as port
                        Console.Clear();
                        break;
                    }
                    catch (FormatException e) // Error for incorrect port
                    {
                        Console.WriteLine($"({DateTime.Now}) ERROR::Please Input Correct Formated Port");
                        Console.WriteLine(e);
                    }
                }
            }
            Console.Title = $"Server {ServerIPEndPoint}";
            #endregion

            #region MAIN
            TcpListener server = new TcpListener(ServerIPEndPoint); //Creates TCPListener on 127.0.0.1:{port}

            #region DNS
            //Region for setting up TCP client and connection to DNS server which creates
            //list of servers
            try
            {
                byte[] DNS_TCPMessage = Encoding.ASCII.GetBytes("SERVER"); //creates ASCII bytes for message
                TcpClient DNSsender = new TcpClient();

                DNSsender.Client.Bind(ServerIPEndPoint); // Bind client to server ip as to add the server ip to list
                DNSsender.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.52"), 9876)); //Connect to DNS

                NetworkStream stream = DNSsender.GetStream();
                stream.Write(DNS_TCPMessage, 0, DNS_TCPMessage.Length); // Write to DNS
                stream.Close(); // Close Stream
                DNSsender.Close(); // Close Client

                Thread UDPthread = new Thread(new ParameterizedThreadStart(ThreadFunctions.HandleUDPClient)); //Create new thread for client UDP server Checks
                UDPthread.Start(ServerIPEndPoint); // Pass server IPEndPoint to bind to
            }
            catch
            {
                Console.WriteLine($"({DateTime.Now}) Fail Connecting to the DNS server");
            }
            #endregion

            server.Start(); // Start TCP listener Server
            Byte[] bytes = new Byte[256]; // Create empty byte array for incoming messages

            

            Console.WriteLine($"({DateTime.Now}):: Started server on: {server.LocalEndpoint}");
            while (true) // Loop for Accepting TCP clients
            {
                TcpClient client = server.AcceptTcpClient(); // when a TCP client joins, it creates a client object for them
                UserCount++;
                Console.Title = $"Server {ServerIPEndPoint} || Total Users {UserCount}"; // Adds user count to the title

                Thread TCPthread = new Thread(new ParameterizedThreadStart(ThreadFunctions.HandleTCPClient)); // Thread for the Clients
                TCPthread.Start(client); // Starts and passes the client
            }
            #endregion
        }
    }

    #region ThreadFunctions
    class ThreadFunctions
    {
        public static void HandleTCPClient(Object Client)
        {
            #region Variables
            TcpClient TCPclient = (TcpClient)Client; // creates TCPclient from the object
            NetworkStream stream = TCPclient.GetStream(); // Get Stream from client
            Byte[] bytes = new Byte[256]; // Creating bytes again
            Byte[] SendBack = null; // Byte array for sending back
            bool close = false;
            int i; // Int for keeping loop
            #endregion

            #region UserIdentification
            Byte[] acknowledgment = Encoding.ASCII.GetBytes("ASK"); // Send request for platform
            stream.Write(acknowledgment, 0, acknowledgment.Length);
            stream.Read(bytes, 0, bytes.Length);
            if (Encoding.ASCII.GetString(bytes).Contains("GUI")) // read and check the platform
            {
                Console.WriteLine($"({DateTime.Now}) User is using GUI");
                Program.ClientsChat.Add(TCPclient); // Add user to list of users that can use chat
            }
            else if (Encoding.ASCII.GetString(bytes).Contains("DOS"))
            {
                Console.WriteLine($"({DateTime.Now}) User is using Console");
            }
            else
            {
                Console.WriteLine($"({DateTime.Now}) Unable to Identify Users platform");
            }
            #endregion

            Console.WriteLine("Made Connection");

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                String ReceivedString = Encoding.ASCII.GetString(bytes, 0, i); // Turns Received bytes into string
                Console.WriteLine($"({DateTime.Now}) Received: {ReceivedString}"); // Writes Received string with timestamp

                Functions.CommandParse(ReceivedString, TCPclient, ref SendBack, ref close); //Parse the commands
                if (close) // checks bool for if have to close
                {
                    Console.WriteLine("Closing Connections");
                    stream.Close();
                    TCPclient.Close();
                    return; // returns therefore closing this thread
                }
                stream.Write(SendBack, 0, SendBack.Length); // Write to stream
                Console.WriteLine($"({DateTime.Now}) Responded : '{Encoding.ASCII.GetString(SendBack)}'"); // log message in console
            }

            TCPclient.Close(); // incase error with stream
        }

        public static void HandleUDPClient(Object ServerEndPointObject)
        {
            #region Client
            UdpClient ServerCheckListener = new UdpClient(); // Creats UDP client used to check if server is alive
            ServerCheckListener.Client.Bind((IPEndPoint)ServerEndPointObject); // Binds UDP client to server IPEndPoint
            ServerCheckListener.Client.ReceiveTimeout = 0; // Sets Client ReceiveTimout to unlimited so it can listen forever
            #endregion


            while (true)
            {
                IPEndPoint from = new IPEndPoint(0, 0); // Creates IPEndPoint that is used to write the from client's IP
                byte[] Request = ServerCheckListener.Receive(ref from); // Waits for Request from clients
                Console.WriteLine($"({DateTime.Now}) {from} is doing servercheck");
                byte[] Response = null;

                if (Encoding.ASCII.GetString(Request).Contains("SERVERCHECK")) // If the Request is SERVERCHECK then ack the check
                {
                    Response = Encoding.ASCII.GetBytes("ACK-SERVERCHECK");
                    ServerCheckListener.Send(Response, Response.Length, from);
                }
                else // Otherwise sendback a identifier
                {
                    Response = Encoding.ASCII.GetBytes($"SERVER-{(IPEndPoint)ServerEndPointObject}");
                    ServerCheckListener.Send(Response, Response.Length, from);
                }
            }
        }
    }
    #endregion
    #region Functions
    class Functions
    {
        public static void CommandParse(string command, TcpClient client,  ref Byte[] SendBack, ref bool close)
        {
            close = false;
            if (command.ToLower().Contains("echo ")) // Checks if Received String is echo
            {
                string echoreponse = command.Remove(0, 5); // removes the echo from the message
                if (echoreponse == "") // If it is nothing
                {
                    SendBack = Encoding.ASCII.GetBytes("You need to type a word to say");
                }
                else
                {
                    SendBack = Encoding.ASCII.GetBytes(echoreponse);
                }
            }
            else if (command.ToLower().Contains("chat ")) // Check if the string is chat
            {
                string ChatMessage = command.Remove(0, 5); // Removes the chat bit
                if (ChatMessage == "") // If its blan message
                {
                    SendBack = Encoding.ASCII.GetBytes("You need to type a word to say");
                }
                else
                {
                    SendBack = Encoding.ASCII.GetBytes($"Sent Message to all clients : {ChatMessage}"); // Sends confirmation that the chat was sent
                    Byte[] ChatMessageSend = Encoding.ASCII.GetBytes(ChatMessage.Insert(0, ";CHAT;")); //  Adds tag to display as message

                    foreach (TcpClient ServerConnectedClients in Program.ClientsChat) // Cycles throught all the clients in list
                    {
                        if (!(ServerConnectedClients == client)) // if the client is the one sending the message, doesn't send to them
                        {
                            NetworkStream temStream = ServerConnectedClients.GetStream(); // New stream with client to send to
                            temStream.Write(ChatMessageSend, 0, ChatMessageSend.Length);
                        }
                    }
                }
            }
            else if (command == "FIN") // Client Disconnecting
            {
                close = true;
                Program.ClientsChat.Remove(client); // Removes client from chat list
                Program.UserCount--;
                Console.Title = $"Server {Program.ServerIPEndPoint} || Total Users {Program.UserCount}";
                return;
            }
            else if (command.ToLower() == "help") // Help command
            {
                SendBack = Encoding.ASCII.GetBytes(@"help - this command
echo - says what you tell it to say (echo {words})
chat - send chat to all clients (chat {words}");
            }
            else // for if the command is unregonised
            {
                SendBack = Encoding.ASCII.GetBytes("Error : not a real command, check spelling");
            }
        }
    }
    #endregion
}
