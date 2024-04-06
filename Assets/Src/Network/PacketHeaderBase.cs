using GameFramework;
using GameFramework.Network;
namespace Dawn{
    public abstract class PacketHeaderBase : IPacketHeader,IReference
    {
        public int PacketLength{
            get;
            set;
        }
        public void Clear()
        {
            PacketLength = 0;
        }
    }
}
