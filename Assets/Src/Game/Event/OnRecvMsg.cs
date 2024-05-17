using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;
using UnityGameFramework.Runtime;
namespace Dawn.Game.Event
{
    public class OnRecvMsg : GameEventArgs
    {
        public static readonly int EventId = typeof(OnRecvMsg).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public bool IsOffline;
        public MsgStruct Msg;

        public override void Clear()
        {
            Msg = null;
            IsOffline = false;
        }
    }
}
