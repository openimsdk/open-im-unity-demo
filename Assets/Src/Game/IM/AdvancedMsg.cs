using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using open_im_sdk.listener;
using open_im_sdk;

namespace Dawn.Game
{
    public class AdvancedMsg : IAdvancedMsgListener
    {
        public void OnMsgDeleted(MsgStruct message)
        {
        }

        public void OnNewRecvMessageRevoked(MessageRevoked messageRevoked)
        {
        }

        public void OnRecvC2CReadReceipt(List<MessageReceipt> msgReceiptList)
        {
        }

        public void OnRecvGroupReadReceipt(List<MessageReceipt> groupMsgReceiptList)
        {
        }

        public void OnRecvMessageExtensionsAdded(string msgID, string reactionExtensionList)
        {
        }

        public void OnRecvMessageExtensionsChanged(string msgID, string reactionExtensionList)
        {
        }

        public void OnRecvMessageExtensionsDeleted(string msgID, string reactionExtensionKeyList)
        {
        }

        public void OnRecvNewMessage(MsgStruct message)
        {
        }

        public void OnRecvOfflineNewMessage(MsgStruct message)
        {
        }

        public void OnRecvOnlineOnlyMessage(MsgStruct message)
        {
        }
    }

}

