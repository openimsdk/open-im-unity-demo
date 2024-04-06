using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnFriendAdd : GameEventArgs
    {
        public static readonly int EventId = typeof(OnFriendAdd).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public List<FullUserInfo> List;


        public override void Clear()
        {
            List.Clear();
        }
    }
}
