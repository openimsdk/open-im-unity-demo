using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public enum GroupOperation
    {
        None,
        GroupApplicationAccepted,
        GroupApplicationAdded,
        GroupApplicationDeleted,
        GroupApplicationRejected,
        GroupDismissed,
        GroupInfoChanged,
        GroupMemberAdded,
        GroupMemberDeleted,
        GroupMemberInfoChanged,
        JoinedGroupAdded,
        JoinedGroupDeleted,
    }

    public class OnGroupChange : GameEventArgs
    {
        public static readonly int EventId = typeof(OnGroupChange).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public GroupOperation Operation;
        //通过会话创建的群聊
        public LocalConversation OldConversation;
        public LocalConversation NewConversation;
        public LocalGroup Group;
        public LocalGroupRequest GroupRequest;
        public LocalGroupMember GroupMemeber;
        public override void Clear()
        {
            Operation = GroupOperation.None;
            OldConversation = null;
            NewConversation = null;
            Group = null;
            GroupRequest = null;
            GroupMemeber = null;
        }

        public bool IsGroupCountChange()
        {
            if (Operation == GroupOperation.GroupDismissed)
            {
                return true;
            }
            if (Operation == GroupOperation.JoinedGroupAdded)
            {
                return true;
            }
            if (Operation == GroupOperation.JoinedGroupDeleted)
            {
                return true;
            }
            return false; ;
        }
    }
}
