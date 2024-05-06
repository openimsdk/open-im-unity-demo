using Dawn.Game.Event;
using open_im_sdk;
using open_im_sdk.listener;


namespace Dawn.Game
{
    public class Group : IGroupListener
    {
        public Group()
        {
        }

        public void OnGroupApplicationAccepted(LocalGroupRequest groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationAccepted,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupApplicationAdded(LocalGroupRequest groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationAdded,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupApplicationDeleted(LocalGroupRequest groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationDeleted,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupApplicationRejected(LocalGroupRequest groupApplication)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupApplicationRejected,
                GroupRequest = groupApplication,
            });
        }

        public void OnGroupDismissed(LocalGroup groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupDismissed,
                Group = groupInfo,
            });
        }

        public void OnGroupInfoChanged(LocalGroup groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupInfoChanged,
                Group = groupInfo,
            });
        }

        public void OnGroupMemberAdded(LocalGroupMember groupMemberInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupMemberAdded,
                GroupMemeber = groupMemberInfo,
            });
        }

        public void OnGroupMemberDeleted(LocalGroupMember groupMemberInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupMemberDeleted,
                GroupMemeber = groupMemberInfo,
            });
        }

        public void OnGroupMemberInfoChanged(LocalGroupMember groupMemberInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.GroupMemberInfoChanged,
                GroupMemeber = groupMemberInfo,
            });
        }

        public void OnJoinedGroupAdded(LocalGroup groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.JoinedGroupAdded,
                Group = groupInfo,
            });
        }

        public void OnJoinedGroupDeleted(LocalGroup groupInfo)
        {
            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
            {
                Operation = GroupOperation.JoinedGroupDeleted,
                Group = groupInfo,
            });
        }
    }
}

