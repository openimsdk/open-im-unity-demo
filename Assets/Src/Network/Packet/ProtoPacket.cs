using System;
namespace Dawn
{
    [Serializable]
    public class ProtoPacket : PacketBase
    {
        public override int Id
        {
            get{
                return 1;
            }
        }
        public static int MsgIdByteCount = sizeof(UInt32);
        public UInt32 MsgID;
        public byte[] Data;

        public override void Clear()
        {
            MsgID = 0;
            Data = null;
        }
    }
}
