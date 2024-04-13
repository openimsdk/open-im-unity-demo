using System.Collections.Generic;
using open_im_sdk;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace Dawn.Game.UI
{
    public class ConversationItem
    {
        public Button Btn;
        public Image Icon;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Time;
        public TextMeshProUGUI Msg;
    }

    public partial class UIMain
    {
        RectTransform conversationRoot;
        LoopListView2 conversationList;
        List<LocalConversation> localConversations;
        void InitConversation()
        {
            conversationRoot = GetRectTransform("Panel/content/center/conversation");
            conversationList = GetListView("Panel/content/center/conversation/list");
            conversationList.InitListView(0, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                if (localConversations == null) return null;
                if (localConversations.Count <= index) return null;
                var info = localConversations[index];
                LoopListViewItem2 itemNode = null;
                itemNode = list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new ConversationItem()
                    {
                        Btn = GetButton("", parent),
                        Icon = GetImage("icon", parent),
                        Name = GetTextPro("name", parent),
                        Time = GetTextPro("time", parent),
                        Msg = GetTextPro("msg", parent)
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                var item = itemNode.UserObjectData as ConversationItem;
                SetConversationItemInfo(item, info);
                return itemNode;
            });
        }
        void OpenConversation()
        {
            conversationList.SetListItemCount(0);
            IMSDK.GetAllConversationList((list, err, errMsg) =>
            {
                if (list != null)
                {
                    localConversations = list;
                    RefreshList(conversationList, localConversations.Count);
                }
                else
                {
                    Debug.LogError(errMsg);
                }
            });

        }
        void CloseConversation()
        {

        }

        void SetConversationItemInfo(ConversationItem item, LocalConversation conversation)
        {
            item.Name.text = conversation.ShowName;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(conversation.LatestMsgSendTime);
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            item.Time.text = localDateTime.ToShortTimeString();
            if (conversation.LatestMsgStruct != null)
            {
                if (conversation.LatestMsgStruct.TextElem != null)
                {
                    item.Msg.text = conversation.LatestMsgStruct.TextElem.Content;
                }
                else
                {
                    item.Msg.text = "";
                }
            }
            else
            {
                item.Msg.text = "";
            }
            OnClick(item.Btn, () =>
            {
                GameEntry.UI.OpenUI("Chat", conversation);
            });
        }
    }
}

