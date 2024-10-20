using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using OpenIM.IMSDK.Unity;
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
        public BlackInfo Black;
        public FriendInfo Friend;
        public FriendApplicationInfo FriendRequest;
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
