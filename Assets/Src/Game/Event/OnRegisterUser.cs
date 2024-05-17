using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
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
