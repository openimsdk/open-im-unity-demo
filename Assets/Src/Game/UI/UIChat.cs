using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;
using Dawn.Game.Event;
using GameFramework.Event;

namespace Dawn.Game.UI
{
    public class UIChat : UGuiForm
    {
        public class ChatItem
        {
            public Button Btn;
            public Image Icon;
            public TextMeshProUGUI Message;
        }
        Button backBtn;
        Button chatInfoBtn;
        TextMeshProUGUI userName;
        LoopListView2 chatList;
        TMP_InputField msgInput;
        List<MsgStruct> msgList;
        RectTransform centerRect;

        LocalConversation conversation;
        LocalUser selfUserInfo;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/content/top/back");
            userName = GetTextPro("Panel/content/top/username");
            chatInfoBtn = GetButton("Panel/content/top/chatinfo");
            chatList = GetListView("Panel/content/center/list");
            msgInput = GetInputField("Panel/content/center/input/input");
            centerRect = GetRectTransform("Panel/content/center");
            msgList = new List<MsgStruct>();
            chatList.InitListView(msgList.Count, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                if (msgList.Count <= index)
                {
                    return null;
                }
                LoopListViewItem2 itemNode = null;
                var info = msgList[index];
                bool isSelf = info.SendID == IMSDK.GetLoginUser();
                if (isSelf)
                {
                    itemNode = list.NewListViewItem("self");
                }
                else
                {
                    itemNode = list.NewListViewItem("friend");
                }
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new ChatItem()
                    {
                        Btn = GetButton("icon", parent),
                        Icon = GetImage("icon", parent),
                        Message = GetTextPro("msg/txt", parent),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                ChatItem item = itemNode.UserObjectData as ChatItem;
                if (isSelf)
                {
                    if (selfUserInfo != null)
                    {
                        SetImage(item.Icon, selfUserInfo.FaceURL);
                    }
                    OnClick(item.Btn, () =>
                    {
                        GameEntry.UI.OpenUI("UserInfo", selfUserInfo.UserID);
                    });
                }
                else
                {
                    SetImage(item.Icon, info.SenderFaceURL);
                    OnClick(item.Btn, () =>
                    {
                        GameEntry.UI.OpenUI("UserInfo", info.SendID);
                    });
                }
                if (info.TextElem != null)
                {
                    item.Message.text = info.TextElem.Content;
                }
                return itemNode;
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            conversation = userData as LocalConversation;
            if (conversation.UnreadCount > 0)
            {
                IMSDK.MarkConversationMessageAsRead((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        Debug.Log("Mark as Read");
                    }
                    else
                    {
                        Debug.Log(errMsg);
                    }
                }, conversation.ConversationID);
            }

            msgInput.onSubmit.RemoveAllListeners();
            msgInput.onSubmit.AddListener((text) =>
            {
                TrySendTextMsg(text);
            });

            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(chatInfoBtn, () =>
            {
                GameEntry.UI.OpenUI("ChatInfo", conversation);
            });
            RefreshUI();
            GameEntry.Event.Subscribe(OnGroupChange.EventId, HandleCreateGroup);
            GameEntry.Event.Subscribe(OnRecvMsg.EventId, HandleOnRecvMsg);
            GameEntry.Event.Subscribe(OnConversationChange.EventId, HandleOnConversationChange);

        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            msgList.Clear();
            selfUserInfo = null;
            centerRect.anchoredPosition = Vector2.zero;
            GameEntry.Event.Unsubscribe(OnGroupChange.EventId, HandleCreateGroup);
            GameEntry.Event.Unsubscribe(OnRecvMsg.EventId, HandleOnRecvMsg);
            GameEntry.Event.Unsubscribe(OnConversationChange.EventId, HandleOnConversationChange);
        }

        void RefreshUI()
        {
            IMSDK.GetSelfUserInfo((userInfo, err, errMsg) =>
            {
                if (userInfo != null)
                {
                    selfUserInfo = userInfo;
                    RefreshList(chatList, msgList.Count);
                }
                else
                {
                    Debug.Log(errMsg);
                }
            });
            msgList.Clear();
            RefreshList(chatList, 0);
            if (conversation != null)
            {
                userName.text = conversation.ShowName;
                IMSDK.GetAdvancedHistoryMessageList((list, err, msg) =>
                {
                    if (list != null)
                    {
                        msgList.Clear();
                        foreach (var msgStruct in list.MessageList)
                        {
                            msgList.Add(msgStruct);
                        }
                    }
                    RefreshList(chatList, msgList.Count);
                }, new GetAdvancedHistoryMessageListParams()
                {
                    UserID = conversation.UserID,
                    ConversationID = conversation.ConversationID,
                    Count = 10,
                });
            }
        }

        void TrySendTextMsg(string value)
        {
            Debug.Log("Try SendTextMsg : " + value);
            if (value == "")
            {
                GameEntry.UI.Tip(" text is empty");
                return;
            }
            var msgStruct = IMSDK.CreateTextMessage(value);
            IMSDK.SendMessage((msg, errCode, errMsg) =>
            {
                if (msg != null)
                {
                    msgList.Add(msg);
                    RefreshList(chatList, msgList.Count);
                    chatList.MovePanelToItemIndex(msgList.Count, 0);
                }
                else
                {
                    Debug.LogError(errCode + "" + errMsg);
                }
            }, msgStruct, conversation.UserID, conversation.GroupID, new OfflinePushInfo()
            {
            });
            msgInput.text = "";
        }

        void HandleCreateGroup(object sender, GameEventArgs e)
        {
            var args = e as OnGroupChange;
            if (args.OldConversation != null && args.NewConversation != null && args.OldConversation.ConversationID == conversation.ConversationID)
            {
                conversation = args.NewConversation;
                RefreshUI();
            }
        }

        void HandleOnRecvMsg(object sender, GameEventArgs e)
        {
            var args = e as OnRecvMsg;
            var msg = args.Msg;
            if (msg != null)
            {
                if (conversation.ConversationType == (int)ConversationType.Single)
                {
                    if (conversation.UserID == msg.SendID)
                    {
                        msgList.Add(msg);
                        RefreshList(chatList, msgList.Count);
                    }
                }
                else if (conversation.ConversationType == (int)ConversationType.Group)
                {
                    if (conversation.GroupID == msg.GroupID)
                    {
                        msgList.Add(msg);
                        RefreshList(chatList, msgList.Count);
                    }
                }
            }
        }

        void HandleOnConversationChange(object sender, GameEventArgs e)
        {
            var args = e as OnConversationChange;
            if (args.Conversation != null && args.Conversation != null && args.Conversation.ConversationID == conversation.ConversationID)
            {
                if (args.ClearHistory)
                {
                    msgList.Clear();
                    RefreshList(chatList, msgList.Count);
                }
            }
        }

    }
}

