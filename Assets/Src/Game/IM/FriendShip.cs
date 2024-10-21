
using System.Collections.Generic;
using Dawn.Game.Event;
using OpenIM.IMSDK.Unity;
using OpenIM.IMSDK.Unity.Listener;


namespace Dawn.Game
{
    public class FriendShip : IFriendShipListener
    {
        public FriendShip()
        {
        }
        public void OnBlackAdded(BlackInfo blackInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Black = blackInfo,
                Operation = FriendOperation.BlackedAdd
            });
        }

        public void OnBlackDeleted(BlackInfo blackInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Black = blackInfo,
                Operation = FriendOperation.BlackDeleted
            });
        }

        public void OnFriendAdded(FriendInfo friendInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Friend = friendInfo,
                Operation = FriendOperation.Added
            });
        }

        public void OnFriendDeleted(FriendInfo friendInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Friend = friendInfo,
                Operation = FriendOperation.Deleted
            });
        }

        public void OnFriendInfoChanged(FriendInfo friendInfo)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                Friend = friendInfo,
                Operation = FriendOperation.InfoChanged
            });
        }

        public void OnFriendApplicationAccepted(FriendApplicationInfo friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationAccepted
            });
        }

        public void OnFriendApplicationAdded(FriendApplicationInfo friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationAdded
            });
        }

        public void OnFriendApplicationDeleted(FriendApplicationInfo friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationDeleted
            });
        }

        public void OnFriendApplicationRejected(FriendApplicationInfo friendApplication)
        {
            GameEntry.Event.Fire(OnFriendChange.EventId, new OnFriendChange()
            {
                FriendRequest = friendApplication,
                Operation = FriendOperation.ApplicationRejected
            });
        }
    }
}

