using Microsoft.FlightSimulator.SimConnect;
using MSFS_Con;
using MSFS_Con.Serial;
using System;
using System.Collections.Generic;
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
            TDebug.WriteLine(simVar + ":" + value);

            SerialMsgConverter.VariableData variables = SerialMsgConverter.ToVariables(simVar, value);
            if( null != variables.simVar)
            {
                String command = variables.ToCommand();
                this._serialProvider?.SendText(command);
            }

            // 変数を受信してUDPで送信するならこのあたり
            // UDP.sendData(hoge);
        }

        /// <summary>
        /// Receive a event
        /// </summary>
        public event Action<Object, String> SimConnect_OnRecvEventEvent;
        private void SimConnect_OnRecvEvent(Object sender, EVENTS recEvent)
        {
            this.SimConnect_OnRecvEventEvent?.Invoke(sender, Enum.GetName(typeof(EVENTS), recEvent));
            TDebug.WriteLine("OnRecvEvent : " + Enum.GetName(typeof(EVENTS), recEvent));
        }

        /// <summary>
        /// 適当に作るならこんな感じ
        /// </summary>
        public void push()
        {
            this.SimConnect_SendData(VARIABLES.PLANE_ALTITUDE, 10000.0f);
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

        #endregion

    }
}
