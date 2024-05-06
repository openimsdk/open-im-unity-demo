using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public enum SyncServerStatus
    {
        Empty, Start, Failed, Finish
    }

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
        public bool Created;
        public bool ClearHistory;
        public LocalConversation Conversation;
        public SyncServerStatus SyncServerStatus = SyncServerStatus.Empty;
        public override void Clear()
        {
            Conversation = null;
            SyncServerStatus = SyncServerStatus.Empty;
            Created = false;
            ClearHistory = false;
        }
    }
}
