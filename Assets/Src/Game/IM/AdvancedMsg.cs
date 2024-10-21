using System.Collections;
using System.Collections.Generic;
using OpenIM.IMSDK.Unity.Listener;
using OpenIM.IMSDK.Unity;
using Dawn.Game.Event;

namespace Dawn.Game
{
    public class AdvancedMsg : IAdvancedMsgListener
    {
        public void OnMsgDeleted(Message message)
        {
            GameEntry.Event.Fire(OnAdvancedMsg.EventId, new OnAdvancedMsg()
            {
                AdvancedMsgOperation = AdvancedMsgOperation.Deleted,
                Msg = message,
            });
        }

        public void OnNewRecvMessageRevoked(MessageRevoked messageRevoked)
        {

            GameEntry.Event.Fire(OnAdvancedMsg.EventId, new OnAdvancedMsg()
            {
                AdvancedMsgOperation = AdvancedMsgOperation.Revoked,
                MsgRevoked = messageRevoked,
            });
        }

        public void OnRecvC2CReadReceipt(List<MessageReceipt> msgReceiptList)
        {

            GameEntry.Event.Fire(OnAdvancedMsg.EventId, new OnAdvancedMsg()
            {
                AdvancedMsgOperation = AdvancedMsgOperation.C2CReadReceipt,
                MsgReceipts = msgReceiptList,
            });
        }

        public void OnRecvGroupReadReceipt(List<MessageReceipt> groupMsgReceiptList)
        {
            GameEntry.Event.Fire(OnAdvancedMsg.EventId, new OnAdvancedMsg()
            {
                AdvancedMsgOperation = AdvancedMsgOperation.Deleted,
                MsgReceipts = groupMsgReceiptList,
            });
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

        public void OnRecvNewMessage(Message message)
        {
        }

        public void OnRecvOfflineNewMessage(Message message)
        {
        }

        public void OnRecvOnlineOnlyMessage(Message message)
        {
        }
    }

}

