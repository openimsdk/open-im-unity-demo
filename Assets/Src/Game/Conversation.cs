using System.Collections.Generic;
using open_im_sdk;
using open_im_sdk.listener;
using UnityEngine;

namespace Dawn.Game
{
    public class Conversation : IConversationListener
    {
        Dictionary<string, LocalConversation> conversationDic;

        public Conversation()
        {
            conversationDic = new Dictionary<string, LocalConversation>();
        }

        public LocalConversation GetFriendConversation(string userId)
        {
            if (conversationDic.ContainsKey(userId))
            {
                return conversationDic[userId];
            }
            else
            {
                return null;
            }
        }

        public void OnConversationChanged(List<LocalConversation> conversationList)
        {
            foreach (var conversation in conversationList)
            {
                if (conversationDic.ContainsKey(conversation.UserID))
                {
                    conversationDic.Remove(conversation.UserID);
                }
                conversationDic[conversation.UserID] = conversation;
            }
        }

        public void OnNewConversation(List<LocalConversation> conversationList)
        {
            foreach (var conversation in conversationList)
            {
                if (conversationDic.ContainsKey(conversation.UserID))
                {
                    conversationDic.Remove(conversation.UserID);
                }
                conversationDic[conversation.UserID] = conversation;
            }
        }
        public void OnSyncServerStart()
        {
        }
        public void OnSyncServerFailed()
        {

        }
        public void OnSyncServerFinish()
        {
            IMSDK.GetAllConversationList((List<LocalConversation> list, int errCode, string errMsg) =>
            {
                if (list != null)
                {
                    foreach (var conversation in list)
                    {
                        if (conversationDic.ContainsKey(conversation.UserID))
                        {
                            conversationDic.Remove(conversation.UserID);
                        }
                        conversationDic[conversation.UserID] = conversation;
                    }
                }
                else
                {
                    Debug.Log(errCode + errMsg);
                }
            });
        }

        public void OnTotalUnreadMessageCountChanged(int totalUnreadCount)
        {

        }

        public void OnConversationUserInputStatusChanged(InputStatesChangedData data)
        {
        }
    }
}

