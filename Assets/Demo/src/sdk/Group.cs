using open_im_sdk;
using open_im_sdk.listener;

public class Group : IGroupListener
{
    public Group()
    {

    }

    public void OnGroupApplicationAccepted(LocalGroupRequest groupApplication)
    {
    }

    public void OnGroupApplicationAdded(LocalGroupRequest groupApplication)
    {
    }

    public void OnGroupApplicationDeleted(LocalGroupRequest groupApplication)
    {
    }

    public void OnGroupApplicationRejected(LocalGroupRequest groupApplication)
    {
    }

    public void OnGroupDismissed(LocalGroup groupInfo)
    {
    }

    public void OnGroupInfoChanged(LocalGroup groupInfo)
    {
    }

    public void OnGroupMemberAdded(LocalGroupMember groupMemberInfo)
    {
    }

    public void OnGroupMemberDeleted(LocalGroupMember groupMemberInfo)
    {
    }

    public void OnGroupMemberInfoChanged(LocalGroupMember groupMemberInfo)
    {
    }

    public void OnJoinedGroupAdded(LocalGroup groupInfo)
    {
    }

    public void OnJoinedGroupDeleted(LocalGroup groupInfo)
    {
    }
}