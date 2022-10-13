using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Con
{
    internal class UDPProvider
    {
        private UDP _udp { get; set; }
        private List<LUDPPeerStruct> _ludpPeerStruct = new List<LUDPPeerStruct>();

        private Int32 LUDPPacketHeaderSizeCache { get; set; }
        private Int32 LUDPDatagramHeaderSizeCache { get; set; }

        public UDPProvider()
        {
            _udp = new UDP();
            _udp.CreateSocket(0);

            this.UDPProviderInit();
        }
        public UDPProvider(Int32 port)
        {
            _udp = new UDP();
            _udp.CreateSocket(port);

            this.UDPProviderInit();
        }
        private void UDPProviderInit()
        {
            _udp.ReceiveUdpDataEvent += this.OnReceiveDataEvent;

            this.LUDPPacketHeaderSizeCache = this.LUDPPacketHeaderSize;
            this.LUDPDatagramHeaderSizeCache = this.LUDPDatagramHeaderSize;
        }

        public void SetServer(String ipAddress, Int32 port)
        {
            LUDPPeerStruct server = new LUDPPeerStruct();
            server.Peer = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            this._ludpPeerStruct.Add(server);
        }

        public void SendData(String message)
        {
            foreach(LUDPPeerStruct v in this._ludpPeerStruct)
            {
                this._udp.SendData(message, v.Peer);
            }
        }
        public void SendData(Byte[] data)
        {
            foreach(LUDPPeerStruct v in this._ludpPeerStruct)
            {
                this._udp.SendData(data, v.Peer); 
            }
        }

        public void OnReceiveDataEvent(Object sender, Byte[] data, IPEndPoint peer)
        {
            // セッション管理は別途要検討
            if(this._ludpPeerStruct.Exists(v => v.Peer.Address.Equals(peer.Address)) == false)
            {
                LUDPPeerStruct client = new LUDPPeerStruct();
                client.Peer = new IPEndPoint(peer.Address, peer.Port);
                this._ludpPeerStruct.Add(client);
            }

            String message = Encoding.UTF8.GetString(data);

            LUDPPacket hd = this.ByteToLUDPPacket(data);

      
            Debug.WriteLine(message);
        }

        class LUDPPeerStruct
        {
            public IPEndPoint Peer { get; set; }
        }


        public byte[] LUDPPacketToByte(LUDPPacket data)
        {
            Int32 index = 0;

            //Calc LUDP packet size. Header + Datagram
            Int32 dtSize = this.LUDPPacketHeaderSizeCache;
            Int32 gSize = this.LUDPDatagramHeaderSizeCache;
            foreach(LUDPDatagram v in data.Data)
            {
                dtSize += gSize + v.DataSize;
            }
            
            byte[] buf = new byte[dtSize];

            buf[index++] = data.Flags;
            buf[index++] = data.DataCount;
            buf[index++] = (Byte)((data.AckNo >> 8) & 0xFF);
            buf[index++] = (Byte)(data.AckNo & 0xFF);

            foreach (LUDPDatagram v in data.Data)
            {
                buf[index++] = (Byte)((v.SessionID >> 8) & 0xFF);
                buf[index++] = (Byte)(v.SessionID & 0xFF);
                buf[index++] = (Byte)((v.ChannelID >> 8) & 0xFF);
                buf[index++] = (Byte)(v.ChannelID & 0xFF);
                buf[index++] = (Byte)((v.SequenceNo >> 8) & 0xFF);
                buf[index++] = (Byte)(v.SequenceNo & 0xFF);
                buf[index++] = (Byte)((v.AckNo >> 8) & 0xFF);
                buf[index++] = (Byte)(v.AckNo & 0xFF);
                buf[index++] = (Byte)((v.DataSize >> 8) & 0xFF);
                buf[index++] = (Byte)(v.DataSize & 0xFF);
                for(int i=0;i < v.Data.Length; i++)
                {
                    buf[index++] = v.Data[i];
                }
            }

            return buf;
        }

        public LUDPPacket ByteToLUDPPacket(byte[] buf)
        {
            Int32 index = 0;

            LUDPPacket data = new LUDPPacket();
            data.Flags = buf[index++];
            data.DataCount = buf[index++];
            data.AckNo = (UInt16)(buf[index++] << 8);
            data.AckNo |= (UInt16)buf[index++];

            data.Data = new LUDPDatagram[data.DataCount];

            for(int i=0; i < data.DataCount; i++)
            {
                LUDPDatagram dt = new LUDPDatagram();
                dt.SessionID = (UInt16)(buf[index++] << 8);
                dt.SessionID |= (UInt16)buf[index++];
                dt.ChannelID = (UInt16)(buf[index++] << 8);
                dt.ChannelID |= (UInt16)buf[index++];
                dt.SequenceNo = (UInt16)(buf[index++] << 8);
                dt.SequenceNo |= (UInt16)buf[index++];
                dt.AckNo = (UInt16)(buf[index++] << 8);
                dt.AckNo |= (UInt16)buf[index++];
                dt.DataSize = (UInt16)(buf[index++] << 8);
                dt.DataSize |= (UInt16)buf[index++];
                byte[] d = new byte[dt.DataSize];
                for(int j=0; j<dt.DataSize; j++)
                {
                    d[j] = buf[index++];
                }
                dt.Data = d;
                data.Data[i] = dt;
            }

            return data;
        }

        private Int32 LUDPPacketHeaderSize 
        {
            get
            {
                Int32 size = 0;
                Type t = typeof(LUDPPacket);
                PropertyInfo[] mem = t.GetProperties();
                foreach(PropertyInfo m in mem)
                {
                    if (m.PropertyType == typeof(LUDPDatagram[])) break;
                    size += Marshal.SizeOf(m.PropertyType);
                }
                return size;
            }
        }
        private Int32 LUDPDatagramHeaderSize
        {
            get
            {
                Int32 size = 0;
                Type t = typeof(LUDPDatagram);
                PropertyInfo[] mem = t.GetProperties();
                foreach (PropertyInfo m in mem)
                {
                    if (m.PropertyType == typeof(Byte[])) break;
                    size += Marshal.SizeOf(m.PropertyType);
                }
                return size;
            }
        }
    }

    public class LUDPPacket
    {
        // Header
        public Byte Flags { get; set; }
        public Byte DataCount { get; set; }
        public UInt16 AckNo { get; set; }
        // Datagram
        public LUDPDatagram[] Data { get; set; }
    }

    public class LUDPDatagram
    {
        // Header
        public UInt16 SessionID { get; set; }
        public UInt16 ChannelID { get; set; }
        public UInt16 SequenceNo { get; set; }
        public UInt16 AckNo { get; set; }
        public UInt16 DataSize { get; set; }
        // Datagram
        public Byte[] Data { get; set; }
    }
}
