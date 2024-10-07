using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using OpenIM.IMSDK.Unity;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnConnStatusChange : GameEventArgs
    {
        public static readonly int EventId = typeof(OnConnStatusChange).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public ConnStatus ConnStatus;

        public override void Clear()
        {
            ConnStatus = ConnStatus.Empty;
        }
    }
}
