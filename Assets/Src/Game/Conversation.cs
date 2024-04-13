using System.Collections.Generic;
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
        }

        public void OnNewConversation(List<LocalConversation> conversationList)
        {
        }
        public void OnSyncServerStart()
        {
        }
        public void OnSyncServerFailed()
        {

        }
        public void OnSyncServerFinish()
        {

        }

        public void OnTotalUnreadMessageCountChanged(int totalUnreadCount)
        {

        }

        public void OnConversationUserInputStatusChanged(InputStatesChangedData data)
        {
        }
    }
}

