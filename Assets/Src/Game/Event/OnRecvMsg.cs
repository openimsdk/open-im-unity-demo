using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using OpenIM.IMSDK.Unity;
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
        public Message Msg;

        public override void Clear()
        {
            Msg = null;
            IsOffline = false;
        }
    }
}
