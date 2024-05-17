using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnLoginStatusChange : GameEventArgs
    {
        public static readonly int EventId = typeof(OnLoginStatusChange).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public UserStatus UserStatus;

        public override void Clear()
        {
            UserStatus = UserStatus.NoLogin;
        }
    }
}
