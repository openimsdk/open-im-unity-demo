using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public enum FriendOperation
    {
        None, Added, Deleted, InfoChanged, BlackedAdd, BlackDeleted, ApplicationAccepted, ApplicationAdded, ApplicationDeleted, ApplicationRejected
    }
    public class OnFriendChange : GameEventArgs
    {
        public static readonly int EventId = typeof(OnFriendChange).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public LocalBlack Black;
        public LocalFriend Friend;
        public LocalFriendRequest FriendRequest;
        public FriendOperation Operation;
        public override void Clear()
        {
            Friend = null;
            Black = null;
            FriendRequest = null;
            Operation = FriendOperation.None;
        }
    }
}
