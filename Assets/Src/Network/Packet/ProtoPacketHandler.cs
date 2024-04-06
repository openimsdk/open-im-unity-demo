using System;
using System.Text;
using System.IO;
using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Dawn
{
    public delegate void ReciveMessage(UInt32 msgId, byte[] data);

    public class ProtoPacketHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return 1;
            }
        }
        public ReciveMessage OnReciveMessage;
        public override void Handle(object sender, Packet packet)
        {
            ProtoPacket packetImpl = packet as ProtoPacket;
            if (OnReciveMessage != null)
            {
                OnReciveMessage(packetImpl.MsgID, packetImpl.Data);
            }
        }
    }
}
