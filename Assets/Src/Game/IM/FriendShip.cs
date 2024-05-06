
using System.Collections.Generic;
using Dawn.Game.Event;
using open_im_sdk;
using open_im_sdk.listener;


namespace Dawn.Game
{
    public class FriendShip : IFriendShipListener
    {
        public FriendShip()
        {
        }
        public void OnBlackAdded(LocalBlack blackInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Black = blackInfo,
                Operation = FriendOperation.BlackedAdd
            });
        }

        public void OnBlackDeleted(LocalBlack blackInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Black = blackInfo,
                Operation = FriendOperation.BlackDeleted
            });
        }

        public void OnFriendAdded(LocalFriend friendInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Friend = friendInfo,
                Operation = FriendOperation.Added
            });
        }

        public void OnFriendDeleted(LocalFriend friendInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Friend = friendInfo,
                Operation = FriendOperation.Deleted
            });
        }

        public void OnFriendInfoChanged(LocalFriend friendInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Friend = friendInfo,
                Operation = FriendOperation.InfoChanged
            });
        }

        public void OnFriendApplicationAccepted(LocalFriendRequest friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationAccepted
            });
        }

        public void OnFriendApplicationAdded(LocalFriendRequest friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationAdded
            });
        }

        public void OnFriendApplicationDeleted(LocalFriendRequest friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationDeleted
            });
        }

        public void OnFriendApplicationRejected(LocalFriendRequest friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationRejected
            });
        }
    }
}

