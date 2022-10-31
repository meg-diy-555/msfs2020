using Microsoft.FlightSimulator.SimConnect;
using MSFS_Con;
using MSFS_Con.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Con
{
    class Controller
    {
        #region Singleton
        private static readonly Lazy<Controller> _instance = new Lazy<Controller>(() => new Controller());
        public static Controller Instance => _instance.Value;


        private Controller() 
        {
            SimConnectProvider.Instance.OnRecvSimobjectDataEvent += this.SimConnect_OnRecvSimobjectData;
            SimConnectProvider.Instance.OnRecvEventEvent += this.SimConnect_OnRecvEvent;
        }
        ~Controller() { }
        #endregion


        #region SimConnect
        public const int WM_USER_SIMCONNECT = SimConnectProvider.WM_USER_SIMCONNECT;

        public void SimConnect_ToggleConnect()
        {
            SimConnectProvider.Instance.ToggleConnect();
        }
        public void SimConnect_SetWindowHandle(IntPtr handle)
        {
            SimConnectProvider.Instance.SetWindowHandle(handle);
            
        }
        public void SimConnect_ReceiveMessage()
        {
            SimConnectProvider.Instance.ReceiveMessage();
        }
        private void SimConnect_SendData(EVENTS e, UInt32 flag)
        {
            SimConnectProvider.Instance.SendData(e, flag);
        }
        private void SimConnect_SendData(Val v, Double data)
        {
            SimConnectProvider.Instance.SendData(v, data);
        }

        public event Action<Object, String, String> SimConnect_OnRecvSimobjectDataEvent;
        /// <summary>
        /// Receive a variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void SimConnect_OnRecvSimobjectData(Object sender, String simVar, String value)
        {
            this.SimConnect_OnRecvSimobjectDataEvent?.Invoke(sender, simVar, value);
            //TDebug.WriteLine(simVar + "@" + value);

            SerialMsgConverter.VariableData variables = SerialMsgConverter.ToVariables(simVar, value);
            if( null != variables.simVar)
            {
                String command = variables.ToCommand();
                this._serialProvider?.SendText(command);
            }

            // 変数を受信してUDPで送信するならこのあたり
            if (this._udpProvider?.IsServer == true)
            {
                this._udpProvider?.SetStreamData(simVar + "@" + value);
            }
        }

        /// <summary>
        /// Receive a event
        /// </summary>
        public event Action<Object, String, String> SimConnect_OnRecvEventEvent;
        private void SimConnect_OnRecvEvent(Object sender, EVENTS recEvent, UInt32 data)
        {
            this.SimConnect_OnRecvEventEvent?.Invoke(sender, Enum.GetName(typeof(EVENTS), recEvent), data.ToString());
            //TDebug.WriteLine("OnRecvEvent : " + Enum.GetName(typeof(EVENTS), recEvent) + " Value:" + data.ToString());

            this._udpProvider?.SetEventData(Enum.GetName(typeof(EVENTS), recEvent) + "@" + data.ToString());
        }

        /// <summary>
        /// 適当に作るならこんな感じ
        /// </summary>
        public void push()
        {
            //this.SimConnect_SendData(VARIABLES.PLANE_ALTITUDE, 10000.0f);
        }
        
        #endregion



        #region Serial
        private SerialProvider _serialProvider = null;

        public String Serial_ComPortName { get { return SerialConfig.com_name; } set { SerialConfig.com_name = value; } }
        public bool? Serial_IsConnect {  get { return this._serialProvider?.IsOpen; } }

        public event Action<Object, String> Serial_ConnectEvent;
        public event Action<Object> Serial_DisconnectEvent;
        public String Serial_ToggleConnect()
        {
            string msg = "";
            if (this._serialProvider == null)
            {
                this._serialProvider = new SerialProvider();
                this._serialProvider.OnDataReceivedEvent += this.Serial_OnDataReceived;
                msg = this._serialProvider.Open();
                this.Serial_ConnectEvent?.Invoke(this, msg);
            }
            else
            {
                this._serialProvider.OnDataReceivedEvent -= this.Serial_OnDataReceived;
                msg = this._serialProvider.Close();
                this._serialProvider = null;
                this.Serial_DisconnectEvent?.Invoke(this);
            }
            return msg;
        }

        public event Action<Object, String> Serial_OnDataReceivedEvent;
        /// <summary>
        /// OnDataReceived event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="message">Received message</param>
        private void Serial_OnDataReceived(Object sender, String message)
        {
            //Formなどで通知を受け取るなら必要
            this.Serial_OnDataReceivedEvent?.Invoke(sender, message);

            SerialMsgConverter.EventData event_data = SerialMsgConverter.ToEvents(message);
            //EVENTの送信例
            this.SimConnect_SendData(event_data.events, event_data.value);

            //Variablesの送信例
            //this.SimConnect_SendData(VARIABLES.PLANE_ALTITUDE, 10000.0f);
        }


        #endregion

        #region UDP
        private UDPProvider _udpProvider = null;

        public void UDP_StartServer()
        {
            if (this._udpProvider is null) this._udpProvider = new UDPProvider(9000);
            this._udpProvider.UDPProvider_OnRetreveEventDataEvent += this.UDP_OnRetreveEventData;
        }

        public void UDP_StartClient(String serverIP, Int32 port)
        {
            if (this._udpProvider is null) this._udpProvider = new UDPProvider();
            this._udpProvider?.SetServer(serverIP, port);

            this._udpProvider.UDPProvider_OnRetreveStreamDataEvent += this.UDP_OnRetreveStreamData;
            this._udpProvider.UDPProvider_OnRetreveEventDataEvent += this.UDP_OnRetreveEventData;
            
        }

        /// <summary>
        /// Receive peer's variables in order to set variables for MSFS2020.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void UDP_OnRetreveStreamData(Object sender, String message)
        {
            //Debug.WriteLine("Receive : " + message);

            //要リファクタリング
            String[] ar = message.Split('@');

            if (ar[0] == "GEAR HANDLE POSITION"
                || ar[0] == "PLANE ALTITUDE"
                || ar[0] == "PLANE HEADING DEGREES TRUE"
                || ar[0] == "PLANE LATITUDE"
                || ar[0] == "PLANE LONGITUDE"
                || ar[0] == "PLANE BANK DEGREES"
                || ar[0] == "PLANE PITCH DEGREES"
                || ar[0] == "ROTATION ACCELERATION BODY X"
                || ar[0] == "ROTATION ACCELERATION BODY Y"
                || ar[0] == "ROTATION ACCELERATION BODY Z"
                || ar[0] == "VELOCITY BODY X"
                || ar[0] == "VELOCITY BODY Y"
                || ar[0] == "VELOCITY BODY Z"
                || ar[0] == "AUTOPILOT ALTITUDE SLOT INDEX"
                || ar[0] == "AUTOPILOT HEADING SLOT INDEX"
                || ar[0] == "AUTOPILOT SPEED SLOT INDEX"
                || ar[0] == "AUTOPILOT VS SLOT INDEX"
                || ar[0] == "AUTOPILOT ALTITUDE LOCK VAR"
                || ar[0] == "AUTOPILOT BANK HOLD REF"
                || ar[0] == "AUTOPILOT FLIGHT LEVEL CHANGE"
                || ar[0] == "AUTOPILOT HEADING LOCK DIR"
                || ar[0] == "AUTOPILOT HEADING MANUALLY TUNABLE"
                || ar[0] == "AUTOPILOT THROTTLE MAX THRUST"
                || ar[0] == "AUTOPILOT VERTICAL HOLD VAR"
                || ar[0] == "AILERON POSITION"
                || ar[0] == "AILERON TRIM PCT"
                || ar[0] == "ELEVATOR POSITION"
                || ar[0] == "ELEVATOR TRIM POSITION"
                || ar[0] == "FLAP POSITION SET"
                || ar[0] == "FLAPS HANDLE INDEX"
                || ar[0] == "RUDDER POSITION"
                || ar[0] == "RUDDER TRIM PCT"
                || ar[0] == "SPOILERS HANDLE POSITION"
                || ar[0] == "FUEL TANK CENTER QUANTITY"
                || ar[0] == "GENERAL ENG THROTTLE LEVER POSITION:1"
                || ar[0] == "GENERAL ENG THROTTLE LEVER POSITION:2"
                )
            {
                SimConnectProvider.Instance.SendData(ar[0], Double.Parse(ar[1]));
            }
        }


        //public event Action<Object, String> UDP_OnRetreveEventDataEvent;
        /// <summary>
        /// Receive peer's event in order to set event for MSFS2020.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void UDP_OnRetreveEventData(Object sender, String message)
        {
            EVENTS e;
            Enum.TryParse(message, out e);

            String[] ar = message.Split('@');
            this.SimConnect_SendData((EVENTS)Enum.Parse(typeof(EVENTS),ar[0]), uint.Parse(ar[1]));

            switch (e)
            {
                case EVENTS.FLAPS_INCR:
                    break;

                default:
                    break;
            }
        }
        #endregion

    }
}
