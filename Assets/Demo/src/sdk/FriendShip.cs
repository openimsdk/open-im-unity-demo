
using System.Collections.Generic;
using System.Reflection;
using open_im_sdk;
using open_im_sdk.listener;
public class FriendShip : IFriendShipListener
{
    public List<LocalFriend> FriendList;
    public List<LocalFriendRequest> RequestList;
    public List<LocalFriendRequest> ApplicationList;
    public FriendShip()
    {
        FriendList = new List<LocalFriend>();
        RequestList = new List<LocalFriendRequest>();
        ApplicationList = new List<LocalFriendRequest>();
    }

    public void AddLocalFriend(LocalFriend localFriend)
    {
        FriendList.Add(localFriend);
    }


    public void OnBlackAdded(LocalBlack blackInfo)
    {
    }

    public void OnBlackDeleted(LocalBlack blackInfo)
    {
    }

    public void OnFriendAdded(LocalFriend friendInfo)
    {
    }

    public void OnFriendApplicationAccepted(LocalFriendRequest friendApplication)
    {
    }

    public void OnFriendApplicationAdded(LocalFriendRequest friendApplication)
    {
    }

    public void OnFriendApplicationDeleted(LocalFriendRequest friendApplication)
    {
    }

    public void OnFriendApplicationRejected(LocalFriendRequest friendApplication)
    {
    }

    public void OnFriendDeleted(LocalFriend friendInfo)
    {
    }

    public void OnFriendInfoChanged(LocalFriend friendInfo)
    {
    }
}