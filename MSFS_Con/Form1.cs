using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using static MSFS_Con.UDPProvider;

namespace MSFS_Con
{

    public partial class Form1 : Form
    {
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
        /// Override WndProc to receive Window message and execute the necessary methods when a message arrives.
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
            Controller.Instance.UDP_StartServer();
        }

        private void ReceiveEventData(object sender, String message)
        {
            /*
            // Sample for invoke UI thread.
            Invoke((Action)(() =>
            {
                TDebug.WriteLine("Received data : " + message);
            }));
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller.Instance.UDP_StartClient(textBox1.Text, 9000);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RTB_DebugWindow.Text = "";
        }



    }
}
