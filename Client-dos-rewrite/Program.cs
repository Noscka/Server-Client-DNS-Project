using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using ObjectForDNSserver;

namespace Client_dos_rewrite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";

            Functions.RelistOptions(Variables.SelectionsCount);
            Menu.EntryMenu();
        }

        
    }

    class Menu
    {
        static Thread ArrowControls = new Thread(new ParameterizedThreadStart(Threads.Myass));
        public static void EntryMenu()
        {
            ArrowControls.Start();
        }

        public static void SearchMenu()
        {
            Variables.SelectionsCount = 0;
            Functions.RelistOptions(Variables.SelectionsCount);
            Thread DNSserverlistGet = new Thread(new ParameterizedThreadStart(Threads.DNSserverGet));
            Thread SearchingTitleThread = new Thread(new ParameterizedThreadStart(Threads.SearchingTitle));
            
            DNSserverlistGet.Start(SearchingTitleThread);
            SearchingTitleThread.Start();
        }

        public static void ConnectMenu()
        {
            Variables.SelectionsCount = 0;


        }

        public static void ConnectedServerMenu()
        {
            int Bottom = Console.WindowTop + Console.WindowHeight - 1;

            Variables.SelectionsCount = 0;
            Variables.DisableSelection = true;
            Variables.Options.Clear();

            Functions.RelistOptions(Variables.SelectionsCount);
            NetworkStream stream = Variables.Client.GetStream();

            Console.Clear();
            Console.WriteLine("Connections Made");
            

            while (true)
            {
                //int x = Console.CursorLeft;
                //int y = Console.CursorTop;
                Console.CursorTop = Bottom;
                Console.CursorLeft = 0;
                Functions.ClearCurrentConsoleLine();
                Console.Write($">>>");
                Byte[] Input = Encoding.ASCII.GetBytes(Console.ReadLine());
                //Console.SetCursorPosition(x, y);
                stream.Write(Input, 0, Input.Length);
            }
        }
    }

    class Functions
    {
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            //Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void RelistOptions(int SelectionCount)
        {
            Console.Clear();
            if (Variables.SettingsMode)
            {
                foreach(SettingsObject setting in Variables.settings)
                {
                    if (setting.SettingTitle == Variables.settings[SelectionCount].SettingTitle)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($">{setting.SettingTitle}<");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(setting.SettingTitle);
                    }
                }
            }
            else
            {
                foreach (OptionsObject option in Variables.Options)
                {
                    if (option.OptionTitle == Variables.Options[SelectionCount].OptionTitle)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($">{option.OptionTitle}<");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(option.OptionTitle);
                    }
                }
            }
        }

        public static bool ActiveServerCheck(IPEndPoint endPoint)
        {
            byte[] TestRequest = Encoding.ASCII.GetBytes("SERVERCHECK");
            IPEndPoint from = new IPEndPoint(0, 0);

            UdpClient testclient = new UdpClient();
            testclient.Client.ReceiveTimeout = 2000;
            testclient.Send(TestRequest, TestRequest.Length, endPoint);
            try
            {
                byte[] RequestReponse = testclient.Receive(ref from);
                if (Encoding.ASCII.GetString(RequestReponse).Contains("ACK-SERVERCHECK"))
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            catch
            {
                return (false);
            }
        }

        public static void Connect(IPEndPoint endpointip)
        {
            int timeout = 0;

            try
            {
                Variables.Client.Connect(endpointip);
                Identification(Variables.Client);
                Thread connectedMenuThread = new Thread(Menu.ConnectedServerMenu);
                Thread ListenThread = new Thread(Threads.ListenThread);
                ListenThread.Name = "Listen Thread";
                connectedMenuThread.Name = "Connected Menu";
                ListenThread.Start();
                connectedMenuThread.Start();
            }
            catch
            {
                if (timeout == 5)
                {
                    Console.WriteLine("Can't Connect");
                }
                timeout++;
            }
        }

        public static void Identification(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            Byte[] ID = Encoding.ASCII.GetBytes("DOS");
            Byte[] Receivingshake = new Byte[256];
            String responsestring = String.Empty;

            Int32 responsebytes = stream.Read(Receivingshake, 0, Receivingshake.Length);
            responsestring = Encoding.ASCII.GetString(Receivingshake, 0, responsebytes);

            if (responsestring == "ASK")
            {
                stream.Write(ID, 0, ID.Length);
            }
        }
    }

    class Threads
    {
        public static void SearchingTitle(object obj)
        {
            while (true) {
                Console.Title = @"Searching |";
                Thread.Sleep(100);
                Console.Title = @"Searching \";
                Thread.Sleep(100);
                Console.Title = @"Searching —";
                Thread.Sleep(100);
                Console.Title = @"Searching /";
                Thread.Sleep(100);
                Console.Title = @"Searching |";
                Thread.Sleep(100);
            }
        }

        public static void DNSserverGet(object searchingthreadobj)
        {
            Thread searchingthread = (Thread)searchingthreadobj;
            Variables.Options.Clear();
            try
            {
                byte[] DNSRequest = Encoding.ASCII.GetBytes("GET");
                byte[] bytes = new byte[256];

                TcpClient getclient = new TcpClient();
                IPEndPoint DNSIP = new IPEndPoint(IPAddress.Parse("192.168.1.52"), 9876);
                getclient.Connect(DNSIP);
                NetworkStream stream = getclient.GetStream();
                stream.Write(DNSRequest, 0, DNSRequest.Length);

                BinaryFormatter bf = new BinaryFormatter();
                List<DNSserverObject> IPEndPointList = (List<DNSserverObject>)bf.Deserialize(stream);

                foreach (DNSserverObject Server in IPEndPointList)
                {
                    if (Functions.ActiveServerCheck(new IPEndPoint(IPAddress.Parse(Server.ipaddress), Server.port)))
                    {
                        Variables.Options.Add(new OptionsObject($"{Server.ipaddress}:{Server.port}", () => Functions.Connect(new IPEndPoint(IPAddress.Parse(Server.ipaddress), Server.port))));
                    }
                    else
                    {
                        byte[] UpdateListSend = Encoding.ASCII.GetBytes($"LISTUPDATE-{Server.ipaddress}:{Server.port}");

                        UdpClient DNSserverlistuptade = new UdpClient();
                        DNSserverlistuptade.Send(UpdateListSend, UpdateListSend.Length, DNSIP);
                    }
                }
            }
            catch (SocketException)
            {
                Variables.Options.Add(new OptionsObject("Error, No DNS server found", ()=>Console.Beep()));
                Console.Beep();
            }
            Variables.Options.Add(new OptionsObject("Refresh", () => Menu.SearchMenu()));
            Functions.RelistOptions(Variables.SelectionsCount);
            searchingthread.Abort();
        }

        public static void Myass(object obj)
        {
            ConsoleKeyInfo cki;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);

                    if (!Variables.DisableSelection)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (!(Variables.SelectionsCount <= 0))
                                {
                                    Variables.SelectionsCount--;
                                }
                                Functions.RelistOptions(Variables.SelectionsCount);
                                break;
                            case ConsoleKey.DownArrow:
                                if (Variables.SettingsMode)
                                {
                                    if (!(Variables.SelectionsCount >= Variables.settings.Count - 1))
                                    {
                                        Variables.SelectionsCount++;
                                    }
                                }
                                else
                                {
                                    if (!(Variables.SelectionsCount >= Variables.Options.Count - 1))
                                    {
                                        Variables.SelectionsCount++;
                                    }
                                }
                                Functions.RelistOptions(Variables.SelectionsCount);
                                break;
                            case ConsoleKey.LeftArrow:
                                if (Variables.SettingsMode)
                                {
                                    Variables.SelectionsCount = Variables.settings.Count - 1;
                                }
                                else
                                {
                                    Variables.SelectionsCount = Variables.Options.Count - 1;
                                }
                                Functions.RelistOptions(Variables.SelectionsCount);
                                break;
                            case ConsoleKey.RightArrow:
                                Variables.SelectionsCount = 0;
                                Functions.RelistOptions(Variables.SelectionsCount);
                                break;
                            case ConsoleKey.Enter:
                                if (!Variables.SettingsMode)
                                {
                                    Variables.Options[Variables.SelectionsCount].OptionAction();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.Write(Variables.settings[Variables.SelectionsCount].SettingQuestion);
                                    Variables.settings[Variables.SelectionsCount].SettingInput = Console.ReadLine();
                                    Functions.RelistOptions(Variables.SelectionsCount);
                                    Console.WriteLine(Variables.settings[Variables.SelectionsCount].SettingInput);
                                }
                                break;
                            case ConsoleKey.F1:
                                if (Variables.SettingsMode) { Variables.SettingsMode = false; }
                                else { Variables.SettingsMode = true; }
                                Functions.RelistOptions(Variables.SelectionsCount);
                                break;
                        }
                    }
                    else
                    {
                        if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                        {
                            Variables.DisableSelection = false;
                        }
                    }
                }
            }
        }

        public static void ListenThread()
        {
            Byte[] Response = new byte[256];
            int x, y;
            NetworkStream stream = Variables.Client.GetStream();

            while (true)
            {
                Int32 ChatBytes = stream.Read(Response, 0, Response.Length);
                x = Console.CursorLeft;
                y = Console.CursorTop;
                Console.SetCursorPosition(0, Variables.currentCursorLine);
                Console.WriteLine(Encoding.ASCII.GetString(Response));
                Console.SetCursorPosition(x, y);
                Variables.currentCursorLine++;
            }
        }
    }

    class Variables
    {
        public static List<OptionsObject> Options = new List<OptionsObject> { new OptionsObject("Search", ()=>Menu.SearchMenu()), new OptionsObject("Connect", () => Menu.ConnectMenu()) };
        public static List<SettingsObject> settings = new List<SettingsObject> { new SettingsObject("Username", "What do you want your username to be", "") };
        public static int SelectionsCount = 0, currentCursorLine = 1;
        public static TcpClient Client = new TcpClient();
        public static bool DisableSelection = false, SettingsMode = false;
    }

    class OptionsObject
    {
        public String OptionTitle { get; }
        public Action OptionAction { get; }
        public OptionsObject(String optionTitle, Action optionAction)
        {
            OptionTitle = optionTitle;
            OptionAction = optionAction;
        }
    }
    class SettingsObject
    {
        public String SettingTitle { get; }
        public String SettingQuestion { get; }
        public String SettingInput { get; set; }
        public SettingsObject(String settingtitle,String settingquestion, String settinginput)
        {
            SettingTitle = settingtitle;
            SettingQuestion = settingquestion;
            SettingInput = settinginput;
        }
    }
}
