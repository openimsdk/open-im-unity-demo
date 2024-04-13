using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnLogout : GameEventArgs
    {
        public static readonly int EventId = typeof(OnLogout).GetHashCode();
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
