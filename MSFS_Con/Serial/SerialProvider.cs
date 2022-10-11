using System;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;

namespace MSFS_Con
{
    class SerialProvider : SerialPort
    {
        public SerialProvider() : base(
            SerialConfig.com_name,
            SerialConfig.com_baudrate,
            Parity.None,
            8,
            StopBits.One)
        {
            Handshake = Handshake.None;
            Encoding = Encoding.UTF8;
            WriteTimeout = 100000;
            ReadTimeout = 100000;
            NewLine = "\r\n";
        }

        public new string Open()
        {
            if (!base.IsOpen)
            {
                try
                {
                    base.DataReceived += this.SerialPort_DataReceived;
                    base.Open();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return ex.Message;
                }
            }
            return "Connection to " + SerialConfig.com_name + " has been established.";
        }

        private const char newline = '\n';
        private string str_msg = string.Empty;


        public event Action<Object, String> OnDataReceivedEvent;
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort serialport = (SerialPort)sender;
            int read_bytes = serialport.BytesToRead;
            if (read_bytes > 0)
            {
                string msg = serialport.ReadLine();
                Console.WriteLine("RECV:" + msg);
                SendText("Echo: " + msg);

                OnDataReceivedEvent.Invoke(this, msg);
            }
        }

        public void SendText(string msg)
        {
            base.Write(msg + "\r\n");
        }

        public new string Close()
        {
            if (base.IsOpen)
            {
                try
                {
                    base.DataReceived -= this.SerialPort_DataReceived;
                    base.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return ex.Message;
                }
            }
            return "Connection to " + SerialConfig.com_name + " has been disconnected.";
        }
    }
}