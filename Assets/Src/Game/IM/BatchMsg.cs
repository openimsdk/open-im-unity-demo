using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using open_im_sdk.listener;
using open_im_sdk;
using Dawn.Game.Event;

namespace Dawn.Game
{
    public class BatchMsg : IBatchMsgListener
    {
        public void OnRecvNewMessages(List<MsgStruct> messageList)
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

        public void OnRecvOfflineNewMessages(List<MsgStruct> messageList)
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

