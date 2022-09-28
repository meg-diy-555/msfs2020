using System;
using System.Windows.Forms;

using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;


namespace MSFS_Con
{
    enum EVENTS
    {
        //Autopilot Event
        ALTITUDE_SLOT_INDEX_SET,
        HEADING_SLOT_INDEX_SET,
        SPEED_SLOT_INDEX_SET,
        VS_SLOT_INDEX_SET,
    };

    enum NOTIFICATION_GROUPS
    {
        GROUP0,
    }

    class Controller
    {
        // Instance
        static private Controller _controller = null;

        // Const
        public const int WM_USER_SIMCONNECT = 0x0402;
        // Window(Form) handle
        private IntPtr m_hWnd = new IntPtr(0);
        // SimConnect
        private SimConnect m_oSimConnect = null;

        ~Controller()
        {
            Disconnect();
        }

        public static Controller getController()
        {
            if (null == _controller)
            {
                _controller = new Controller();
            }
            return _controller;
        }


        public void SetWindowHandle(IntPtr _hWnd)
        {
            m_hWnd = _hWnd;
        }


        private void Connect()
        {
            try
            {
                // Map client event to sim event (Autopilot)
                m_oSimConnect = new SimConnect("Simconnect - Simvar test", m_hWnd, WM_USER_SIMCONNECT, null, 0);
                m_oSimConnect.MapClientEventToSimEvent(EVENTS.ALTITUDE_SLOT_INDEX_SET, "ALTITUDE_SLOT_INDEX_SET");
                m_oSimConnect.AddClientEventToNotificationGroup(NOTIFICATION_GROUPS.GROUP0, EVENTS.ALTITUDE_SLOT_INDEX_SET, false);
                m_oSimConnect.MapClientEventToSimEvent(EVENTS.HEADING_SLOT_INDEX_SET, "HEADING_SLOT_INDEX_SET");
                m_oSimConnect.AddClientEventToNotificationGroup(NOTIFICATION_GROUPS.GROUP0, EVENTS.HEADING_SLOT_INDEX_SET, false);
                m_oSimConnect.MapClientEventToSimEvent(EVENTS.SPEED_SLOT_INDEX_SET, "SPEED_SLOT_INDEX_SET");
                m_oSimConnect.AddClientEventToNotificationGroup(NOTIFICATION_GROUPS.GROUP0, EVENTS.SPEED_SLOT_INDEX_SET, false);
                m_oSimConnect.MapClientEventToSimEvent(EVENTS.VS_SLOT_INDEX_SET, "VS_SLOT_INDEX_SET");
                m_oSimConnect.AddClientEventToNotificationGroup(NOTIFICATION_GROUPS.GROUP0, EVENTS.VS_SLOT_INDEX_SET, false);

                // set the group priority
                m_oSimConnect.SetNotificationGroupPriority(NOTIFICATION_GROUPS.GROUP0, SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST);
                Console.WriteLine("Connected to SimConnect.");
            }
            catch (COMException ex)
            {
                Console.WriteLine("Connection to KH failed: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnect");
            if (m_oSimConnect != null)
            {
                m_oSimConnect.Dispose();
                m_oSimConnect = null;
            }
        }

        public void ToggleConnect()
        {
            if (m_oSimConnect == null)
            {
                try
                {
                    Connect();
                }
                catch (COMException ex)
                {
                    Console.WriteLine("Unable to connect to SimConnect: " + ex.Message);
                }
            }
            else
            {
                Disconnect();
            }
        }

    }
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form1 = new Form1();
            Controller.getController().SetWindowHandle(form1.Handle);
            Application.Run(form1);
        }

        
    }
}
