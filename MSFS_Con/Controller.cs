using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MSFS_Con
{
    class Controller
    {
        static private Controller _controller = null;               //Instance
        public const int WM_USER_SIMCONNECT = 0x0402;
        private IntPtr m_hWnd = new IntPtr(0);                      //Form handle
        public SimConnect m_oSimConnect = null;
        private List<SimVars> m_vars = new List<SimVars>();

        public event Action<Object, String> RecvSimobjectDataHandler;

        ~Controller()
        {
            Disconnect();
        }

        public static Controller getController()
        {
            if (null == _controller) _controller = new Controller();
            
            return _controller;
        }

        public void SetWindowHandle(IntPtr _hWnd)
        {
            m_hWnd = _hWnd;
        }

        #region EventHandler
        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        { }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        { }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            Console.WriteLine("SimConnect_OnRecvException: " + eException.ToString());
        }

        /// <summary>
        /// When invoke request at specified timing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            Console.WriteLine("SimConnect_OnRecvSimobjectData");

            List<SimVars> fin = m_vars.Where(e => e.Request == data.dwRequestID).ToList();
            //SimVars sv = m_vars.Where(e => e.Request == data.dwRequestID).First();

            foreach(var v in fin)
            {
                String mes = "";
                mes += v.SimVar + ",";
                if(v.Units == null)
                {
                    mes += ((SimVarString)data.dwData[0]).value;
                }
                else
                {
                    double d = (double)data.dwData[0];
                    mes += d.ToString();
                }
                TDebug.WriteLine(mes);

                this.RecvSimobjectDataHandler?.Invoke(this, mes);

            }
        }

        /// <summary>
        /// When invoke request only once.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            Console.WriteLine("SimConnect_OnRecvSimobjectDataBytype");
        }

        /// <summary>
        /// When received events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recEvent"></param>
        private void SimConnect_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT recEvent)
        {
            TDebug.WriteLine("OnRecvEvent : " + Enum.GetName(typeof(EVENTS), recEvent.uEventID));

            switch (recEvent.uEventID)
            {
                case (uint)EVENTS.FLAPS_INCR:
                    //FLAPS_INCの時の処理を書くならこんな感じ
                    break;
            }
        }
        #endregion

        private void Connect()
        {
            try
            {
                // Instance
                m_oSimConnect = new SimConnect("Simconnect - Simvar test", m_hWnd, WM_USER_SIMCONNECT, null, 0);
                m_vars = SimVars.getVariables();

                // Add EventHandler
                m_oSimConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                m_oSimConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);
                m_oSimConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);
                m_oSimConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);
                m_oSimConnect.OnRecvSimobjectData += new SimConnect.RecvSimobjectDataEventHandler(SimConnect_OnRecvSimobjectData);
                m_oSimConnect.OnRecvEvent += new SimConnect.RecvEventEventHandler(SimConnect_OnRecvEvent);

                // Map EVENT(enum) all
                var values = Enum.GetValues(typeof(EVENTS)).OfType<EVENTS>();
                foreach (EVENTS e in values)
                {
                    m_oSimConnect.MapClientEventToSimEvent(e, e.ToString());    // Map enum and enum Name
                    m_oSimConnect.AddClientEventToNotificationGroup(NOTIFICATION_GROUPS.Group0, e, false);
                }                
                // set the group priority(EVENT)
                m_oSimConnect.SetNotificationGroupPriority(NOTIFICATION_GROUPS.Group0, SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST);

                // Map Variables all
                foreach (var v in m_vars)
                {
                    m_oSimConnect.AddToDataDefinition((DEFINITION)v.Definition, v.SimVar, v.Units, v.SIMCONNECT_DATATYPE, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    if (v.Units == null)
                    {
                        m_oSimConnect.RegisterDataDefineStruct<SimVarString>((DEFINITION)v.Definition);
                    }
                    else
                    { 
                        m_oSimConnect.RegisterDataDefineStruct<double>((DEFINITION)v.Definition);
                    }
                    m_oSimConnect.RequestDataOnSimObject((REQUEST)v.Request, (DEFINITION)v.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT, 0, 0, 0);
                }

                Console.WriteLine("Connected to SimConnect.");
                TDebug.WriteLine("Connected");
            }
            catch (COMException ex)
            {
                Console.WriteLine("Connection to KH failed: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnect");
            TDebug.WriteLine("Disconnected");
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
                catch (Exception ex)
                {
                    Console.WriteLine("Uncaught error: " + ex.Message);
                }
            }
            else
            {
                Disconnect();
            }
        }

        #region Send values and events.
        public void ToHighAlt()
        {
            if (m_oSimConnect == null) return;
            Double d = 8800.0;
            SimVars f = m_vars.Where(e => e.SimVar == "PLANE ALTITUDE").First();

            m_oSimConnect.SetDataOnSimObject((DEFINITION)f.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, d);
        }
        public void SendEventAPMASTER()
        {
            if (m_oSimConnect == null) return;
            m_oSimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.AP_MASTER, 1, NOTIFICATION_GROUPS.Group0, SIMCONNECT_EVENT_FLAG.DEFAULT);
        }
        public void SendEventAPSPDVARSET(int i)
        {
            if (m_oSimConnect == null) return;
            m_oSimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.AP_SPD_VAR_SET, (uint)i, NOTIFICATION_GROUPS.Group0, SIMCONNECT_EVENT_FLAG.DEFAULT);
        }
        public void SendEventAPALTVARSET(int i)
        {
            if (m_oSimConnect == null) return;
            m_oSimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.AP_ALT_VAR_SET_ENGLISH, (uint)i, NOTIFICATION_GROUPS.Group0, SIMCONNECT_EVENT_FLAG.DEFAULT);
        }
        public void SendEventHEADINGBUGSET(int i)
        {
            if (m_oSimConnect == null) return;
            m_oSimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.HEADING_BUG_SET, (uint)i, NOTIFICATION_GROUPS.Group0, SIMCONNECT_EVENT_FLAG.DEFAULT);
        }
        public void SetVariablesDouble(String name, Double data)
        {
            if (m_oSimConnect == null) return;
            SimVars f = m_vars.Where(e => e.SimVar == name).First();
            m_oSimConnect.SetDataOnSimObject((DEFINITION)f.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, data);
        }
        #endregion

        #region Serial Communication
        // Serial Communicator
        private Serial m_oSerial = null;
        public Serial Serial { get { return m_oSerial; } }
        public string ToggleConnectSerial()
        {
            string msg = "";
            if (m_oSerial == null)
            {
                m_oSerial = new Serial();
                msg = m_oSerial.Open();
            }
            else
            {
                msg = m_oSerial.Close();
                m_oSerial = null;
            }
            return msg;
        }
        #endregion
    }

}
