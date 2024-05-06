using System.Collections.Generic;
using Dawn.Game.Event;
using open_im_sdk;
using open_im_sdk.listener;

namespace Dawn.Game
{
    public class Conversation : IConversationListener
    {
        public Conversation()
        {
        }

        public void OnConversationChanged(List<LocalConversation> conversationList)
        {
            if (conversationList != null)
            {
                foreach (var conversation in conversationList)
                {
                    GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange()
                    {
                        Conversation = conversation,
                        Created = false,
                    });
                }
            }
        }

        public void OnNewConversation(List<LocalConversation> conversationList)
        {
            if (conversationList != null)
            {
                foreach (var conversation in conversationList)
                {
                    GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange()
                    {
                        Conversation = conversation,
                        Created = true,
                    });
                }
            }
        }
        public void OnSyncServerStart()
        {
            GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange()
            {
                SyncServerStatus = SyncServerStatus.Start
            });
        }
        public void OnSyncServerFailed()
        {
            GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange()
            {
                SyncServerStatus = SyncServerStatus.Failed
            });
        }
        public void OnSyncServerFinish()
        {
            GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange()
            {
                SyncServerStatus = SyncServerStatus.Finish
            });
        }

        public void OnTotalUnreadMessageCountChanged(int totalUnreadCount)
        {
            GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange()
            {
            });
        }

        public void OnConversationUserInputStatusChanged(InputStatesChangedData data)
        {
        }
    }
}

