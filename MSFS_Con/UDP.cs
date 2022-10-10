using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MSFS_Con
{
    /// <summary>
    /// UDPのサーバとクライアント実装
    /// </summary>
    internal class UDP
    {
        /// <summary>
        /// サーバの場合
        /// StartServerMode(Int32 port) → UDP.ReceiveEventHandler += [関数]
        /// サーバ側は送信する前に一度クライアントからデータを受信しないとデータを送信できない。
        /// （クライアントのアドレスを知らないため。最初に受信したクライアントとのみ通信可能。）
        /// 
        /// クライアントの場合
        /// StartClientMode(String serverIP, Int32 port) → UDP.ReceiveEventHander += [関数]
        /// 
        /// データ送信は
        /// UDP.SendData()
        /// 
        /// データ受信は
        /// UDP.ReceiveEventHanderのイベントハンドラを登録すればデータ受信時に呼ばれる。
        /// </summary>
        private UdpClient udpClient;
        private IPEndPoint remoteEP;
        private Boolean isSetRemoteEP = false;
        private Boolean isStarted = false;
        public Boolean isServer = false;

        //Form側でUIを操作しようとするとスレッドが違うのでエラーになるからForm側でInvokeすること
        public event Action<Object, Byte[]> ReceiveEventDataHandler;

        public UDP()
        {
        }

        public void StartServerMode(Int32 port)
        {
            if (this.isStarted) return;
            IPEndPoint myendpoint = new IPEndPoint(IPAddress.Any, port);
            this.udpClient = new UdpClient(myendpoint);

            this.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), this.udpClient);

            this.isStarted = true;

            this.isServer = true;
        }
        public void StartClientMode(String serverIP, Int32 port)
        {
            if (this.isStarted) return;
            IPEndPoint myendpoint = new IPEndPoint(IPAddress.Any, 0);
            this.udpClient = new UdpClient(myendpoint);

            this.remoteEP = new IPEndPoint(IPAddress.Parse(serverIP), port);
            this.isSetRemoteEP = true;

            this.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), this.udpClient);

            this.isStarted = true;
        }

        public async void SendData(String message)
        {
            if (!this.isSetRemoteEP) return;
            byte[] sendByte = Encoding.UTF8.GetBytes(message);
            await this.udpClient.SendAsync(sendByte, sendByte.Length, this.remoteEP);
        }
        public async void SendData(Byte[] data)
        {
            if (!this.isSetRemoteEP) return;
            await this.udpClient.SendAsync(data, data.Length, this.remoteEP);
        }

        /// <summary>
        /// Use Action<Object, Byte[]> ReceiveEventHander</Object>
        /// </summary>
        /// <param name="ar">UdpClient</param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)(ar.AsyncState);
            try
            {
                IPEndPoint e = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = u.EndReceive(ar, ref e);

                if (!this.isSetRemoteEP)
                {
                    this.remoteEP = e;
                    this.isSetRemoteEP = true;
                }

                this.ReceiveEventDataHandler?.Invoke(this, data);
            }
            ///
            /// このエラー処理どうするか要検討
            ///
            catch(Exception e)
            {
                this.udpClient.Dispose();
                this.udpClient = null;
                IPEndPoint myendpoint = new IPEndPoint(IPAddress.Any, 0);
                this.udpClient = new UdpClient(myendpoint);
            }
            this.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), u);
        }
    }
}
