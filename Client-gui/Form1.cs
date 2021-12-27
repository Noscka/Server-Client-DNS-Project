using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ObjectForDNSserver;

namespace Client_gui
{
    public partial class Form1 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        TcpClient client = null;
        NetworkStream stream = null;
        bool chatEnabled = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ErrorLabel.Visible = false;
            Connectbutton.Enabled = false;
            LoadingPicture.Visible = false;
            MainPanel.Visible = false;
        }
        private void ControlBar_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void ExitPictureBox_MouseHover(object sender, EventArgs e)
        {
            ExitPictureBox.BackColor = Color.FromArgb(240, 71, 71);
        }

        private void ExitPictureBox_MouseLeave(object sender, EventArgs e)
        {
            ExitPictureBox.BackColor = Color.FromArgb(45, 45, 48);
        }

        private void ExitPictureBox_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MinimizePictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MinimizePictureBox_MouseHover(object sender, EventArgs e)
        {
            MinimizePictureBox.BackColor = Color.FromArgb(50, 53, 56);
        }

        private void MinimizePictureBox_MouseLeave(object sender, EventArgs e)
        {
            MinimizePictureBox.BackColor = Color.FromArgb(45, 45, 48);
        }

        private void TheIpInputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TheIpInputTextBox.Text.Length == 0)
            {
                Connectbutton.Enabled = false;
            }
            else
            {
                Connectbutton.Enabled = true;
            }
        }

        private void TheIpInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Connectbutton_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void Connectbutton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Visible = false;
            LoadingPicture.Visible = true;
            try
            {
                IPEndPoint ServerIP = CreateIPEndPoint(TheIpInputTextBox.Text);
                Connection(ServerIP);
            }
            catch (FormatException)
            {
                ErrorLabel.Text = "Format Error";
                ErrorLabel.Visible = true;
            }
        }

        private void ConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Byte[] MessageToSend;
                if (chatEnabled)
                {
                    MessageToSend = Encoding.ASCII.GetBytes($"chat {ConsoleInput.Text}");
                }
                else
                {
                    MessageToSend = Encoding.ASCII.GetBytes(ConsoleInput.Text);
                }
                stream.Write(MessageToSend, 0, MessageToSend.Length);
                ConsoleInput.Text = String.Empty;

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Byte[] TerminationMessage = Encoding.ASCII.GetBytes("FIN");
            stream.Write(TerminationMessage, 0, TerminationMessage.Length);
            stream.Close();
            client.Close();
            Output.Text = String.Empty;
            LoadingPicture.Visible = false;
            MainPanel.Visible = false;
        }

        private void ChatCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ChatCheck.Checked)
            {
                chatEnabled = true;
            }
            else
            {
                chatEnabled = false;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            ServerDisplay.Controls.Clear();
            ServerSearchWorker.RunWorkerAsync();
        }

        #region Functions


        void Connection(IPEndPoint endpointip)
        {
            ErrorLabel.Visible = false;
            int timeout = 0;
            client = new TcpClient();

            try
            {
                client.Connect(endpointip);
                Identification(client);
                object PassingArguments = new object[2] { client, stream };
                Thread t = new Thread(new ParameterizedThreadStart(Listen));
                t.SetApartmentState(ApartmentState.STA);
                t.Start(PassingArguments);
            }
            catch
            {
                if (timeout == 5)
                {
                    ErrorLabel.Text = "Can't Make Connection";
                    ErrorLabel.Visible = true;
                }
                timeout++;
            }
        }

        void Identification(TcpClient client)
        {
            stream = client.GetStream();
            Byte[] Handshake = Encoding.ASCII.GetBytes("GUI");
            Byte[] Receivingshake = new Byte[256];
            String responsestring = String.Empty;

            Int32 responsebytes = stream.Read(Receivingshake, 0, Receivingshake.Length);
            responsestring = Encoding.ASCII.GetString(Receivingshake, 0, responsebytes);

            if (responsestring == "ASK")
            {
                stream.Write(Handshake, 0, Handshake.Length);
                MainPanel.Visible = true;
            }
        }

        private bool ActiveServerCheck(IPEndPoint endPoint)
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

        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-address");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-address");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }

        public void AddToConsole(string value)
        {
            TextWriter _writer = new TextBoxStreamWriter(Output);
            Console.SetOut(_writer);
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AddToConsole), new object[] { value });
                return;
            }
            Console.WriteLine(value);
        }

        private void Listen(object inputObject)
        {
            Byte[] Receiving = new byte[256];
            String ChatString = String.Empty;

            while (true)
            {
                try
                {
                    Int32 ChatBytes = stream.Read(Receiving, 0, Receiving.Length);
                    ChatString = Encoding.ASCII.GetString(Receiving, 0, ChatBytes);
                    if (ChatString.Contains(";CHAT;"))
                    {
                        AddToConsole($"Message>>> {ChatString.Remove(0, 6)}");
                    }
                    else
                    {
                        AddToConsole($">>>{ChatString}");
                    }
                }
                catch
                {
                    return;
                }
            }
        }
        #endregion
        #region BackGroundWorkers
        private void ServerSearchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            byte[] DNSRequest = Encoding.ASCII.GetBytes("GET");
            byte[] bytes = new byte[256];

            TcpClient getclient = new TcpClient();
            IPEndPoint DNSIP = new IPEndPoint(IPAddress.Parse("192.168.1.52"), 9876);
            getclient.Connect(DNSIP);
            NetworkStream stream = getclient.GetStream();
            stream.Write(DNSRequest, 0, DNSRequest.Length);


            BinaryFormatter bf = new BinaryFormatter();
            List<DNSserverObject> IPEndPointList = (List<DNSserverObject>)bf.Deserialize(stream);

            foreach (DNSserverObject SERVER in IPEndPointList)
            {
                if (ActiveServerCheck(new IPEndPoint(IPAddress.Parse(SERVER.ipaddress), SERVER.port)))
                {
                    //Panel
                    Panel panel = new Panel();
                    panel.Size = new Size(184, 45);
                    panel.BackColor = Color.FromArgb(82, 82, 82);
                    panel.Click += Panel_Click;
                    panel.MouseHover += Panel_MouseHover;
                    panel.MouseLeave += Panel_MouseLeave;

                    //Label
                    Label label = new Label();
                    label.Name = "IPlabel";
                    label.ForeColor = Color.White;
                    label.Text = $"{SERVER.ipaddress}:{SERVER.port}";
                    label.Location = new Point(3, 26);
                    label.Size = new Size(178, 16);
                    label.AutoSize = false;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    label.Font = new Font("BankGothic Lt BT", 11F);
                    label.Click += Label_Click;
                    label.MouseHover += Label_MouseHover;
                    label.MouseLeave += Label_MouseLeave;

                    //Add control
                    panel.Controls.Add(label);

                    worker.ReportProgress(10, panel);
                }
                else
                {
                    byte[] UpdateListSend = Encoding.ASCII.GetBytes($"LISTUPDATE-{SERVER.ipaddress}:{SERVER.port}");

                    UdpClient DNSserverlistuptade = new UdpClient();
                    DNSserverlistuptade.Send(UpdateListSend, UpdateListSend.Length, DNSIP);
                }
            }
        }
        private void ServerSearchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Panel panel = (Panel)e.UserState;
            ServerDisplay.Controls.Add(panel);
        }
        #endregion
        #region Dynamic Events
        private void Panel_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            string serverIPstring = String.Empty;
            serverIPstring = panel.Controls["IPlabel"].Text;
            IPEndPoint ServerIP = CreateIPEndPoint(serverIPstring);
            Connection(ServerIP);
        }

        private void Panel_MouseHover(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackColor = Color.FromArgb(62, 62, 62);
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackColor = Color.FromArgb(82, 82, 82);
        }

        private void Label_Click(object sender, EventArgs e)
        {
            Panel_Click(((Control)sender).Parent, e);
        }

        private void Label_MouseHover(object sender, EventArgs e)
        {
            Panel_MouseHover(((Control)sender).Parent, e);
        }

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            Panel_MouseLeave(((Control)sender).Parent, e);
        }
        #endregion
    }
    public class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }

    public class RangeFinder
    {
        public static IEnumerable<string> GetIPRange(IPAddress startIP,
            IPAddress endIP)
        {
            uint sIP = ipToUint(startIP.GetAddressBytes());
            uint eIP = ipToUint(endIP.GetAddressBytes());
            while (sIP <= eIP)
            {
                yield return new IPAddress(reverseBytesArray(sIP)).ToString();
                sIP++;
            }
        }


        /* reverse byte order in array */
        protected static uint reverseBytesArray(uint ip)
        {
            byte[] bytes = BitConverter.GetBytes(ip);
            bytes = bytes.Reverse().ToArray();
            return (uint)BitConverter.ToInt32(bytes, 0);
        }


        /* Convert bytes array to 32 bit long value */
        protected static uint ipToUint(byte[] ipBytes)
        {
            ByteConverter bConvert = new ByteConverter();
            uint ipUint = 0;

            int shift = 24; // indicates number of bits left for shifting
            foreach (byte b in ipBytes)
            {
                if (ipUint == 0)
                {
                    ipUint = (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                    shift -= 8;
                    continue;
                }

                if (shift >= 8)
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                else
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint));

                shift -= 8;
            }

            return ipUint;
        }
    }
}