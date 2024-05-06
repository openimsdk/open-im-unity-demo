using System.Collections.Generic;
using open_im_sdk;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Dawn.Game.Event;
using GameFramework.Event;

namespace Dawn.Game.UI
{

    public partial class UIMain
    {
        class ConversationItem
        {
            public RectTransform Rect;
            public Image Icon;
            public TextMeshProUGUI Name;
            public TextMeshProUGUI Time;
            public TextMeshProUGUI Msg;
            public SwipeButton SwipeBtn;
            public Button PinBtn;
            public Button DeleteBtn;
        }
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
                        Rect = parent,
                        Icon = GetImage("icon", parent),
                        Name = GetTextPro("name", parent),
                        Time = GetTextPro("time", parent),
                        Msg = GetTextPro("msg", parent),
                        SwipeBtn = GetControl(typeof(SwipeButton), "", parent) as SwipeButton,
                        PinBtn = GetButton("menu/pin", parent),
                        DeleteBtn = GetButton("menu/delete", parent),
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
            RefreshConversationList();

            GameEntry.Event.Subscribe(OnConversationChange.EventId, HandleConversationChange);
        }
        void CloseConversation()
        {
            GameEntry.Event.Unsubscribe(OnConversationChange.EventId, HandleConversationChange);
        }

        void RefreshConversationList()
        {
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

        void SetConversationItemInfo(ConversationItem item, LocalConversation conversation)
        {
            item.Name.text = conversation.ShowName;
            SetImage(item.Icon, conversation.FaceURL);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(conversation.LatestMsgSendTime);
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            item.Time.text = localDateTime.ToShortTimeString();
            MsgStruct msg = open_im_sdk.util.Utils.FromJson<MsgStruct>(conversation.LatestMsg);
            if (msg != null && msg.TextElem != null)
            {
                item.Msg.text = msg.TextElem.Content;
            }
            else
            {
                item.Msg.text = "";
            }
            item.SwipeBtn.OnSwipe.RemoveAllListeners();
            item.SwipeBtn.OnSwipe.AddListener((dx, dy) =>
            {
                var pos = item.Rect.anchoredPosition;
                pos.x += dx;
                pos.x = Mathf.Clamp(pos.x, -300, 0);
                item.Rect.anchoredPosition = pos;
            });
            item.SwipeBtn.OnClick.RemoveAllListeners();
            item.SwipeBtn.OnClick.AddListener(() =>
            {
                GameEntry.UI.OpenUI("Chat", conversation);
            });
            OnClick(item.PinBtn, () =>
            {
                GameEntry.UI.Tip("TODO");
            });
            OnClick(item.DeleteBtn, () =>
            {
                IMSDK.DeleteConversationAndDeleteAllMsg((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        RefreshConversationList();
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, conversation.ConversationID);
            });
        }
        void HandleConversationChange(object sender, GameEventArgs e)
        {
            var args = e as OnConversationChange;
            if (args.SyncServerStatus == SyncServerStatus.Finish)
            {
            }
            if (args.Conversation != null)
            {
                RefreshConversationList();
            }
        }
    }
}

