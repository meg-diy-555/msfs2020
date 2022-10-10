using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSFS_Con
{

    public partial class Form1 : Form
    {
        UDP _udp = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller.getController().ToggleConnect();
            if(Controller.getController().m_oSimConnect != null)
            {
                Controller.getController().RecvSimobjectDataHandler += this.ReceiveEventData;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this._udp = new UDP();
            this._udp.ReceiveEventDataHandler += receive;
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
                    //TDebug.WriteLine("Capture WndProc" + m.ToString());
                    Controller.getController().m_oSimConnect?.ReceiveMessage();
                    break;
            }
            base.WndProc(ref m);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            _udp.StartServerMode(9000);
            TDebug.WriteLine("Server Start");
        }

        private void receive(object sender, byte[] data)
        {
            /*
            Invoke((Action)(() =>
            {
                TDebug.WriteLine("受信データ:" +  Encoding.UTF8.GetString(data));
            }));
            */
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
                Controller.getController().SetVariablesDouble(ar[0], Double.Parse(ar[1]));
                TDebug.WriteLine("受信:" + ar[0] + ":" + ar[1]);
            }

        }
        private void ReceiveEventData(object sender, String message)
        {
            if(_udp.isServer)
            {
                _udp.SendData(message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _udp.StartClientMode(textBox1.Text, 9000);
            _udp.SendData("Request Start");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RTB_DebugWindow.Text = "";
        }
    }
}
