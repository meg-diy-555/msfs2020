using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MSFS_Con
{
    class SimConnectProvider
    {


        public const int WM_USER_SIMCONNECT = 0x0402;



        private static readonly Lazy<SimConnectProvider> _instance = new Lazy<SimConnectProvider>(() => new SimConnectProvider());
        public static SimConnectProvider Instance => _instance.Value;

        private SimConnect m_oSimConnect = null;
        private IntPtr m_hWnd = new IntPtr(0);                      //Form handle
        private List<SimVarStructure> m_vars = new List<SimVarStructure>();

        private SimConnectProvider()
        {
            //Create variables structure and add to temp variables list.

            UInt32 uDef = 0;
            UInt32 uReq = 0;

            VARIABLES li = new VARIABLES();

            foreach (var prop in li.GetType().GetProperties())
            {
                Val vs = (Val)prop.GetValue(li, null);

                SimVarStructure v = new SimVarStructure();
                v.Definition = uDef++;
                v.Request = uReq++;
                v.SimVar = vs.Name;
                v.Units = vs.Units;
                v.SIMCONNECT_PERIOD = vs.SIMCONNECT_PERIOD;
                v.SIMCONNECT_DATA_REQUEST_FLAG = vs.SIMCONNECT_DATA_REQUEST_FLAG;

                if (v.Units == null)
                {
                    v.SIMCONNECT_DATATYPE = SIMCONNECT_DATATYPE.STRING256;
                }
                else
                {
                    v.SIMCONNECT_DATATYPE = SIMCONNECT_DATATYPE.FLOAT64;
                }
                m_vars.Add(v);
            }


        }
        ~SimConnectProvider()
        {
            Disconnect();
        }

        public void SetWindowHandle(IntPtr _hWnd)
        {
            m_hWnd = _hWnd;
        }

        #region EventHandler
        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data) { }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data) { }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            Debug.WriteLine("SimConnect_OnRecvException: " + eException.ToString());
        }

        public event Action<Object, String, String> OnRecvSimobjectDataEvent;
        /// <summary>
        /// When invoke request at specified timing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            //Debug.WriteLine("SimConnect_OnRecvSimobjectData");

            List<SimVarStructure> fin = m_vars.Where(e => e.Request == data.dwRequestID).ToList();
            String value = "";

            foreach(var v in fin)
            {
                String mes = v.SimVar + ",";
                if(v.Units == null)
                {
                    mes += ((SimVarString)data.dwData[0]).value;
                    value = ((SimVarString)data.dwData[0]).value;
                }
                else
                {
                    mes += ((Double)data.dwData[0]).ToString();
                    value = ((Double)data.dwData[0]).ToString();
                }

                this.OnRecvSimobjectDataEvent?.Invoke(this, v.SimVar, value);
            }
        }

        public event Action<Object, String> OnRecvSimobjectDataByTypeEvent;
        /// <summary>
        /// When invoke request only once.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            Debug.WriteLine("SimConnect_OnRecvSimobjectDataBytype");
            SimVarStructure sv = m_vars.Where(e => e.Request == data.dwRequestID).First();
            String mes = sv.SimVar + ",";
            if(sv.Units == null)
            {
                mes += ((SimVarString)data.dwData[0]).value;
            }
            else
            {
                mes += ((Double)data.dwData[0]).ToString();
            }

            this.OnRecvSimobjectDataByTypeEvent?.Invoke(this, mes);
        }

        public event Action<Object, EVENTS> OnRecvEventEvent;
        /// <summary>
        /// When received events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recEvent"></param>
        private void SimConnect_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT recEvent)
        {
            this.OnRecvEventEvent?.Invoke(this, (EVENTS)recEvent.uEventID);
            /*
            switch (recEvent.uEventID)
            {
                case (uint)EVENTS.FLAPS_INCR:
                    //FLAPS_INCの時の処理を書くならこんな感じ
                    break;
            }
            */
        }
        #endregion



        private void Connect()
        {
            try
            {
                // Instance
                m_oSimConnect = new SimConnect("Simconnect - Simvar test", m_hWnd, WM_USER_SIMCONNECT, null, 0);                

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
                    Debug.WriteLine("ADD DEF : " + v.Definition.ToString() + " Simvar - " + v.SimVar.ToString());
                    m_oSimConnect.AddToDataDefinition((DEFINITION)v.Definition, v.SimVar, v.Units, v.SIMCONNECT_DATATYPE, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    if (v.Units == null)
                    {
                        m_oSimConnect.RegisterDataDefineStruct<SimVarString>((DEFINITION)v.Definition);
                    }
                    else
                    { 
                        m_oSimConnect.RegisterDataDefineStruct<double>((DEFINITION)v.Definition);
                    }
                    m_oSimConnect.RequestDataOnSimObject((REQUEST)v.Request, (DEFINITION)v.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, v.SIMCONNECT_PERIOD, v.SIMCONNECT_DATA_REQUEST_FLAG, 0, 0, 0);
                }

                Debug.WriteLine("Connected to SimConnect.");
            }
            catch (COMException ex)
            {
                Debug.WriteLine("Connection to KH failed: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            Debug.WriteLine("Disconnect");
            if (m_oSimConnect != null)
            {
                m_oSimConnect.Dispose();
                m_oSimConnect = null;
            }
        }

        public void ReceiveMessage()
        {
            m_oSimConnect?.ReceiveMessage();
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
            SimVarStructure f = m_vars.Where(e => e.SimVar == "PLANE ALTITUDE").First();

            m_oSimConnect.SetDataOnSimObject((DEFINITION)f.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, d);
        }
        public void SendData(EVENTS e, UInt32 flag)
        {
            if (m_oSimConnect == null) return;
            this.m_oSimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, e, flag, NOTIFICATION_GROUPS.Group0, SIMCONNECT_EVENT_FLAG.DEFAULT);
        }
        public void SendData(String name, Double data)
        {
            if (m_oSimConnect == null) return;
            SimVarStructure f = m_vars.Where(e => e.SimVar == name).First();
            m_oSimConnect.SetDataOnSimObject((DEFINITION)f.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, data);
        }
        public void SendData(Val vl, Double data)
        {
            if (m_oSimConnect == null) return;
            SimVarStructure f = m_vars.Where(e => e.SimVar == vl.Name).First();
            m_oSimConnect.SetDataOnSimObject((DEFINITION)f.Definition, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, data);
        }
        #endregion

        #region Inner class and structure


        class SimVarStructure
        {
            public UInt32 Definition { get; set; }
            public UInt32 Request { get; set; }
            public String SimVar { get; set; }
            public String Units { get; set; }
            public SIMCONNECT_DATATYPE SIMCONNECT_DATATYPE { get; set; }
            public SIMCONNECT_PERIOD SIMCONNECT_PERIOD { get; set; }
            public SIMCONNECT_DATA_REQUEST_FLAG SIMCONNECT_DATA_REQUEST_FLAG { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct SimVarString
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String value;
        };
        enum DEFINITION
        {
            def,
        };

        enum REQUEST
        {
            req,
        };
        enum NOTIFICATION_GROUPS
        {
            Group0,
        }

        #endregion
    }
}
