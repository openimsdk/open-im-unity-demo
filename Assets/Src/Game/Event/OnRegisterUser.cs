using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using OpenIM.IMSDK.Unity;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnRegisterUser : GameEventArgs
    {
        public static readonly int EventId = typeof(OnRegisterUser).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string UserID;
        public override void Clear()
        {
            UserID = "";
        }
    }
}
