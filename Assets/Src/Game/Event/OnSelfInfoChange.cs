using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using OpenIM.IMSDK.Unity;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnSelfInfoChange : GameEventArgs
    {
        public static readonly int EventId = typeof(OnSelfInfoChange).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public override void Clear()
        {
        }
    }
}
