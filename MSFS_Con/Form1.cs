using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MSFS_Con
{

    public partial class Form1 : Form
    {
        Dictionary<String, List<String>> _paramPairs = new Dictionary<String, List<String>>();
        DateTime _newestTime = DateTime.Now;

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

            Controller.Instance.SimConnect_OnRecvEventEvent += this.ReceiveEventData;
            Controller.Instance.SimConnect_OnRecvSimobjectDataEvent += this.ReceiveStreamData;
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

        private void ReceiveEventData(object sender, String e, String data)
        {
            if(this._newestTime < DateTime.Now) this._newestTime = DateTime.Now;
            List<String> v = new List<String> { DateTime.Now.ToString("HH:mm:ss"), data };
            this._paramPairs["EVENT: " + e] = v;
            /*
            // Sample for invoke UI thread.
            Invoke((Action)(() =>
            {
                TDebug.WriteLine("Received data : " + e + " data:" + data);
            }));
            */
        }
        private void ReceiveStreamData(object sender, String simvar, String value)
        {
            if (this._newestTime < DateTime.Now) this._newestTime = DateTime.Now;
            List<String> v = new List<String> { DateTime.Now.ToString("HH:mm:ss"), value };
            this._paramPairs["STREM: " + simvar] = v;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller.Instance.UDP_StartClient(textBox1.Text, 9000);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String mes = "";         

            foreach(KeyValuePair<String, List<String>> pair in this._paramPairs.OrderBy( p => p.Key))
            {
                mes += pair.Value[0] + " - " + pair.Key + " @ " + pair.Value[1] + "\n";
            }
            RTB_DebugWindow.Text = mes;

            int pos = 0;
            while(true)
            {
                pos = RTB_DebugWindow.Find(this._newestTime.ToString("HH:mm:ss"), pos, RichTextBoxFinds.None);
                if (pos == -1) break;
                RTB_DebugWindow.SelectionBackColor = Color.Aquamarine;
                pos++;
            }
        }



    }
}
