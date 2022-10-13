using System;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static MSFS_Con.UDPProvider;

namespace MSFS_Con
{

    public partial class Form1 : Form
    {
        UDP _udp = null;
        private UDPProvider _udpProvider { get; set; }

        public Form1()
        {
            InitializeComponent();
            Controller.Instance.SimConnect_SetWindowHandle(this.Handle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller.Instance.SimConnect_ToggleConnect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox_comname.Text = Controller.Instance.Serial_ComPortName;

            this._udp = new UDP();
            this._udp.ReceiveUdpDataEvent += this.ReceiveUdpData;
        }

        private void textBox_comname_TextChanged(object sender, EventArgs e)
        {
            Controller.Instance.Serial_ComPortName = this.textBox_comname.Text;
        }

        private void button_com_connect_Click(object sender, EventArgs e)
        {
            String msg = Controller.Instance.Serial_ToggleConnect();
            if(Controller.Instance.Serial_IsConnect == true)
            {
                this.button_com_connect.Text = "disconnect";
            }
            else
            {
                this.button_com_connect.Text = "connect";
            }
            this.toolStripStatusLabel1.Text = msg;

        }


        /// <summary>
        /// Window messageを受け取り、自分宛てのメッセージが到着している時に必要なメソッドを実行するためにWndProcをオーバーライド
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                // Message to this program
                case Controller.WM_USER_SIMCONNECT:
                    Controller.Instance.SimConnect_ReceiveMessage();
                    break;
            }
            base.WndProc(ref m);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            //_udp.StartServerMode(9000);
            //_udp.CreateSocket(9000);
            //TDebug.WriteLine("Server Start");
            if (this._udpProvider is null) this._udpProvider = new UDPProvider(9000);
        }

        private void ReceiveUdpData(object sender, byte[] data, IPEndPoint e)
        {
            //要リファクタリング

            //_udp.SendData("お返事");
            String message = Encoding.UTF8.GetString(data);

            String[] ar = message.Split(',');

            if (ar[0] == "PLANE ALTITUDE" 
                || ar[0] == "PLANE HEADING DEGREES TRUE" 
                || ar[0] == "PLANE LATITUDE" 
                || ar[0] == "PLANE LONGITUDE" 
                || ar[0] == "PLANE BANK DEGREES" 
                || ar[0] == "PLANE PITCH DEGREES"
                || ar[0] == "GEAR HANDLE POSITION"
                || ar[0] == "AILERON POSITION"
                || ar[0] == "ELEVATOR POSITION"
                || ar[0] == "FLAP POSITION SET"
                || ar[0] == "RUDDER POSITION"
                || ar[0] == "GENERAL ENG THROTTLE LEVER POSITION:1"
                || ar[0] == "GENERAL ENG THROTTLE LEVER POSITION:2"

                )
            {
                SimConnectProvider.Instance.SendData(ar[0], Double.Parse(ar[1]));
                TDebug.WriteLine("受信:" + ar[0] + ":" + ar[1]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //_udp.StartClientMode(textBox1.Text, 9000);
            //_udp.CreateSocket(9000, textBox1.Text);
            //_udp.SendData("Request Start");
            if (this._udpProvider is null)
            {
                this._udpProvider = new UDPProvider();
                this._udpProvider.SetServer(textBox1.Text, 9000);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RTB_DebugWindow.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Controller.Instance.push();
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            //this._udpProvider?.SendData("テスト");
            LUDPHeaderStruct sendheader = new LUDPHeaderStruct();
            int size = Marshal.SizeOf(sendheader);
            sendheader.Flags = 0x0A;
            sendheader.AckNo = 45000;
            sendheader.Tests = 0x03;
            IntPtr pss = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(sendheader, pss, false);
            byte[] sendbyte = new byte[size];
            Marshal.Copy(pss, sendbyte, 0, size);
            Marshal.FreeHGlobal(pss);


            this._udpProvider?.SendData(sendbyte);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
