using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using System;
using System.Text;
using System.IO;
using UnityGameFramework.Runtime;
namespace Dawn
{
    public class NetworkChannelHelper : INetworkChannelHelper
    {
        // private readonly Dictionary<int, Type> m_ServerToClientPacketTypes = new Dictionary<int, Type>();
        private readonly MemoryStream m_CachedStream = new MemoryStream(1024 * 8);
        private INetworkChannel m_NetworkChannel = null;

        public Action CBNetworkConnected = null;
        public Action CBNetworkError = null;
        public Action CBNetworkClosed = null;

        public ReciveMessage OnReciveMessage = null;
        /// <summary>
        /// 获取消息包头长度。
        /// </summary>
        public int PacketHeaderLength
        {
            get
            {
                return sizeof(int);
            }
        }

        /// <summary>
        /// 初始化网络频道辅助器。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        public void Initialize(INetworkChannel networkChannel)
        {
            m_NetworkChannel = networkChannel;
            var protoPacketHeader = new ProtoPacketHandler();
            protoPacketHeader.OnReciveMessage = HandlePacket;
            // 反射注册包和包处理函数。
            m_NetworkChannel.RegisterHandler(protoPacketHeader);

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
        }

        public void HandlePacket(UInt32 msgId, byte[] data)
        {
            if (OnReciveMessage != null)
            {
                OnReciveMessage(msgId, data);
            }
        }

        /// <summary>
        /// 关闭并清理网络频道辅助器。
        /// </summary>
        public void Shutdown()
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);

            if (CBNetworkConnected != null)
            {
                CBNetworkConnected = null;
            }
            if (CBNetworkError != null)
            {
                CBNetworkError = null;
            }
            if (CBNetworkClosed != null)
            {
                CBNetworkClosed = null;
            }
            if (OnReciveMessage != null)
            {
                OnReciveMessage = null;
            }

            m_NetworkChannel = null;
        }

        /// <summary>
        /// 准备进行连接。
        /// </summary>
        public void PrepareForConnecting()
        {
            m_NetworkChannel.Socket.ReceiveBufferSize = 1024 * 64;
            m_NetworkChannel.Socket.SendBufferSize = 1024 * 64;
        }

        /// <summary>
        /// 发送心跳消息包。
        /// </summary>
        /// <returns>是否发送心跳消息包成功。</returns>
        public bool SendHeartBeat()
        {
            // m_NetworkChannel.Send(ReferencePool.Acquire<CSHeartBeat>());
            // return true;
            return false;
        }

        public bool SendProtoPacket(UInt32 msgId, byte[] data)
        {
            // Log.Info(data);
            // Log.Info(data.Length);
            var packet = ReferencePool.Acquire<ProtoPacket>();
            packet.MsgID = msgId;
            // packet.Data = Encoding.BigEndianUnicode.GetBytes(data);
            packet.Data = data;
            // Log.Info(msgId);
            // Log.Info(BitConverter.ToString(packet.Data));
            m_NetworkChannel.Send<ProtoPacket>(packet);
            return true;
        }

        /// <summary>
        /// 序列化消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要序列化的消息包。</param>
        /// <param name="destination">要序列化的目标流。</param>
        /// <returns>是否序列化成功。</returns>
        public bool Serialize<T>(T packet, Stream destination) where T : Packet
        {
            var protoPacket = packet as ProtoPacket;
            byte[] msgid = BitConverter.GetBytes(protoPacket.MsgID);
            byte[] datalength = BitConverter.GetBytes(protoPacket.Data.Length + msgid.Length);
            if (BitConverter.IsLittleEndian)
            { // 改成大端模式
                Array.Reverse(msgid);
                Array.Reverse(datalength);
            }
            destination.Write(datalength, 0, 4);
            destination.Write(msgid, 0, 4);
            if (protoPacket.Data != null && protoPacket.Data.Length > 0)
            {
                destination.Write(protoPacket.Data, 0, protoPacket.Data.Length);
            }
            // Log.Info(BitConverter.ToString(datalength));
            // Log.Info(BitConverter.ToString(msgid));
            // Log.Info(BitConverter.ToString(protoPacket.Data));
            return true;
        }

        /// <summary>
        /// 反序列消息包头。
        /// </summary>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns></returns>
        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            customErrorData = null;
            ProtoPacketHeader header = ReferencePool.Acquire<ProtoPacketHeader>();
            byte[] buff = new byte[source.Length];
            source.Read(buff, 0, sizeof(int));
            //服务器以大端模式发送数据
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buff);
            }
            var dataLength = BitConverter.ToUInt32(buff, 0);
            header.PacketLength = (int)dataLength; //不带包头的数据长度
            return header;
        }

        /// <summary>
        /// 反序列化消息包。
        /// </summary>
        /// <param name="packetHeader">消息包头。</param>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包。</returns>
        public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            customErrorData = null;
            ProtoPacketHeader protoPacketHeader = packetHeader as ProtoPacketHeader;
            //服务器以小端模式发送数据
            Packet packet = null;
            if (protoPacketHeader != null)
            {
                byte[] msgid = new byte[ProtoPacket.MsgIdByteCount];
                var dataLength = protoPacketHeader.PacketLength - ProtoPacket.MsgIdByteCount;
                byte[] data = new byte[protoPacketHeader.PacketLength];
                source.Read(msgid, 0, 4);
                source.Read(data, 0, dataLength);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(msgid);
                }
                var protoPacket = ReferencePool.Acquire<ProtoPacket>();
                protoPacket.MsgID = BitConverter.ToUInt32(msgid, 0);
                protoPacket.Data = data;
                packet = protoPacket;
            }
            else
            {
                Log.Warning("Packet header is invalid.");
            }
            ReferencePool.Release(protoPacketHeader);
            return packet;
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkConnectedEventArgs ne = (UnityGameFramework.Runtime.NetworkConnectedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
            if (CBNetworkConnected != null)
            {
                CBNetworkConnected();
            }
            // Log.Info("Network channel '{0}' connected, local address '{1}', remote address '{2}'.", ne.NetworkChannel.Name, ne.NetworkChannel.Socket.LocalEndPoint.ToString(), ne.NetworkChannel.Socket.RemoteEndPoint.ToString());
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkClosedEventArgs ne = (UnityGameFramework.Runtime.NetworkClosedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
            if (CBNetworkClosed != null)
            {
                CBNetworkClosed();
            }
            // Log.Info("Network channel '{0}' closed.", ne.NetworkChannel.Name);
        }

        private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs ne = (UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' miss heart beat '{1}' times.", ne.NetworkChannel.Name, ne.MissCount.ToString());

            if (ne.MissCount < 2)
            {
                return;
            }

            ne.NetworkChannel.Close();
        }

        private void OnNetworkError(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkErrorEventArgs ne = (UnityGameFramework.Runtime.NetworkErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
            if (CBNetworkError != null)
            {
                CBNetworkError();
            }
            // Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.", ne.NetworkChannel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);

            ne.NetworkChannel.Close();
        }

        private void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkCustomErrorEventArgs ne = (UnityGameFramework.Runtime.NetworkCustomErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }
        }
    }
}
