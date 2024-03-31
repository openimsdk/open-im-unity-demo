using System.Collections.Generic;
using open_im_sdk;
using open_im_sdk.listener;
using UnityEngine;
public class Conversation : IConversationListener
{
    List<LocalConversation> conversationList;
    public List<LocalConversation> ConversationList
    {
        get
        {
            return conversationList;
        }
    }

    public Conversation()
    {
        conversationList = new List<LocalConversation>();
    }

    public void AddConversationListRange(List<LocalConversation> list)
    {
        foreach (var newConversation in list)
        {
            int oldConvertionIndex = -1;
            for (int i = 0; i < conversationList.Count; i++)
            {
                if (conversationList[i].ConversationID == newConversation.ConversationID)
                {
                    oldConvertionIndex = i;
                    break;
                }
            }
            if (oldConvertionIndex >= 0)
            {
                conversationList[oldConvertionIndex] = newConversation;
            }
            else
            {
                conversationList.Add(newConversation);
            }
        }
    }

    public LocalConversation GetFriendConversation(string userId)
    {
        foreach (var con in conversationList)
        {
            if (con.UserID == userId)
            {
                return con;
            }
        }
        return null;
    }

    public void OnConversationChanged(List<LocalConversation> conversationList)
    {
        AddConversationListRange(conversationList);
        foreach (var converation in conversationList)
        {
            Player.CurPlayer.Dispator.Broadcast(EventType.OnConversationChange, converation);
        }
    }

    public void OnNewConversation(List<LocalConversation> conversationList)
    {
    }
    public void OnSyncServerStart()
    {
    }
    public void OnSyncServerFailed()
    {
        Player.CurPlayer.Dispator.Broadcast(EventType.OnConversationSyncFailed);
    }
    public void OnSyncServerFinish()
    {
        IMSDK.GetAllConversationList((List<LocalConversation> list, int errCode, string errMsg) =>
        {
            if (list != null)
            {
                Debug.Log("ConversationList Count = " + list.Count);
                AddConversationListRange(list);
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