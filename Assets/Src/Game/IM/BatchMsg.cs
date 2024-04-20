using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using open_im_sdk.listener;
using open_im_sdk;

namespace Dawn.Game
{
    public class BatchMsg : IBatchMsgListener
    {
        public void OnRecvNewMessages(List<MsgStruct> messageList)
        {
        }

        public void OnRecvOfflineNewMessages(List<MsgStruct> messageList)
        {

        }
    }

}

