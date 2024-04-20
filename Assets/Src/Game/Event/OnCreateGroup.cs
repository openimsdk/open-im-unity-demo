using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnCreateGroup : GameEventArgs
    {
        public static readonly int EventId = typeof(OnCreateGroup).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public LocalConversation OldConversation;
        public LocalConversation NewConversation;
        public LocalGroup Group;

        public override void Clear()
        {
        }
    }
}
