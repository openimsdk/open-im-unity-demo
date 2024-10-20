using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenIM.IMSDK.Unity.Listener;
using OpenIM.IMSDK.Unity;
using Dawn.Game.Event;

namespace Dawn.Game
{
    public class BatchMsg : IBatchMsgListener
    {
        public void OnRecvNewMessages(List<Message> messageList)
        {
            if (messageList != null)
            {
                foreach (var msg in messageList)
                {
                    GameEntry.Event.Fire(OnRecvMsg.EventId, new OnRecvMsg()
                    {
                        Msg = msg,
                        IsOffline = false,
                    });
                }
            }
        }

        public void OnRecvOfflineNewMessages(List<Message> messageList)
        {
            if (messageList != null)
            {
                foreach (var msg in messageList)
                {
                    GameEntry.Event.Fire(OnRecvMsg.EventId, new OnRecvMsg()
                    {
                        Msg = msg,
                        IsOffline = true,
                    });
                }
            }
        }
    }
}

