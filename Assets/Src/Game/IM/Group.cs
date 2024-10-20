using Dawn.Game.Event;
using OpenIM.IMSDK.Unity;
using OpenIM.IMSDK.Unity.Listener;


namespace Dawn.Game
{
    public class Group : IGroupListener
    {
        public Group()
        {
        }

        public void OnGroupApplicationAccepted(GroupApplicationInfo groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationAccepted,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupApplicationAdded(GroupApplicationInfo groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationAdded,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupApplicationDeleted(GroupApplicationInfo groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationDeleted,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupApplicationRejected(GroupApplicationInfo groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationRejected,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupDismissed(GroupInfo groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupDismissed,
                Group = groupInfo,
            });
        }

        public void OnGroupInfoChanged(GroupInfo groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupInfoChanged,
                Group = groupInfo,
            });
        }

        public void OnGroupMemberAdded(GroupMember groupMemberInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupMemberAdded,
                GroupMemeber = groupMemberInfo,
            });
        }

        public void OnGroupMemberDeleted(GroupMember groupMemberInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupMemberDeleted,
                GroupMemeber = groupMemberInfo,
            });
        }

        public void OnGroupMemberInfoChanged(GroupMember groupMemberInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupMemberInfoChanged,
                GroupMemeber = groupMemberInfo,
            });
        }

        public void OnJoinedGroupAdded(GroupInfo groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.JoinedGroupAdded,
                Group = groupInfo,
            });
        }

        public void OnJoinedGroupDeleted(GroupInfo groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.JoinedGroupDeleted,
                Group = groupInfo,
            });
        }
    }
}

