using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnConversationChange : GameEventArgs
    {
        public static readonly int EventId = typeof(OnConversationChange).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public List<LocalFriend> List;


        public override void Clear()
        {
            List.Clear();
        }
    }
}
