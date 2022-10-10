using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSFS_Con
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller.getController().ToggleConnect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox_comname.Text = Config.com_name;
        }

        private void textBox_comname_TextChanged(object sender, EventArgs e)
        {
            Config.com_name = this.textBox_comname.Text;
        }

        private void button_com_connect_Click(object sender, EventArgs e)
        {
            string msg = Controller.getController().ToggleConnectSerial();
            Serial serial = Controller.getController().Serial;
            if (null != serial && serial.IsOpen)
            {
                this.button_com_connect.Text = "disconnect";
            }
            else
            {
                this.button_com_connect.Text = "connect";
            }
            this.toolStripStatusLabel1.Text = msg;
        }
    }
}
