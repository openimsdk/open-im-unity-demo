using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public enum AdvancedMsgOperation
    {
        None, Deleted, Revoked, C2CReadReceipt, GroupReadReceipt
    }
    public class OnAdvancedMsg : GameEventArgs
    {
        public static readonly int EventId = typeof(OnAdvancedMsg).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public AdvancedMsgOperation AdvancedMsgOperation;
        public MsgStruct Msg;
        public MessageRevoked MsgRevoked;
        public List<MessageReceipt> MsgReceipts;
        public override void Clear()
        {
            Msg = null;
            MsgRevoked = null;
            MsgReceipts = null;
        }
    }
}
