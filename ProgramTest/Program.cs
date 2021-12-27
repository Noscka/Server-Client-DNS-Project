using System;
using System.Diagnostics;
using System.Threading;

namespace ProgramTest
{
    class Program
    {
        static void Main(string[] args)
        {
            String ServerAmount, ClientAmount, DNSbool, IPrange;

            Thread TitleChanger = new Thread(new ParameterizedThreadStart(TitleLoop));
            TitleChanger.Start();
            
            Console.Write("How many Servers do you want (int): ");
            ServerAmount = Console.ReadLine();
            Console.Write("Port range for the Servers (int-int): ");
            IPrange = Console.ReadLine();
            Console.Write("How Many Clients do you want (int): ");
            ClientAmount = Console.ReadLine();
            Console.Write("Do you want DNS server (y/n): ");
            DNSbool = Console.ReadLine().ToLower();

            if (DNSbool == "y" | DNSbool == "yes")
            {
                Process.Start(@"D:\Users\Adam\Documents\Programming Projects\Visual Studio Projects\Csharp\Network\DNS\bin\Debug\DNS.exe");
            }

            Thread.Sleep(1000);

            for (int i = 0; i < int.Parse(ServerAmount); i++)
            {
                Random rnd = new Random();
                int port = rnd.Next(int.Parse(IPrange.GetUntilOrEmpty()), int.Parse(IPrange.Substring(IPrange.LastIndexOf('-') + 1)));

                ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Users\Adam\Documents\Programming Projects\Visual Studio Projects\Csharp\Network\Server Rewrite\bin\Debug\Server Rewrite.exe");
                startInfo.Arguments = port.ToString();
                Process.Start(startInfo);
            }

            for (int i = 0; i < int.Parse(ClientAmount); i++)
            {
                Process.Start(@"D:\Users\Adam\Documents\Programming Projects\Visual Studio Projects\Csharp\Network\Client-gui\bin\Debug\Client-gui.exe");
            }
        }

        static void TitleLoop(object obj)
        {
            while (true)
            {
                Console.Title = @"Testing |";
                Thread.Sleep(100);
                Console.Title = @"Testing \";
                Thread.Sleep(100);
                Console.Title = @"Testing —";
                Thread.Sleep(100);
                Console.Title = @"Testing /";
                Thread.Sleep(100);
                Console.Title = @"Testing |";
                Thread.Sleep(100);
            }
        }
    }

    static class Helper
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    }
}
