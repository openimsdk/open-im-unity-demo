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
            public Image Icon;
            public TextMeshProUGUI Message;
        }
        Button backBtn;
        Button chatInfoBtn;
        TextMeshProUGUI userName;
        LoopListView2 chatList;
        TMP_InputField inputMsg;
        Button sendBtn;
        List<MsgStruct> msgList;
        LocalConversation conversation;
        LocalUser selfUserInfo;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/content/top/back");
            userName = GetTextPro("Panel/content/top/username");
            chatInfoBtn = GetButton("Panel/content/top/chatinfo");
            chatList = GetListView("Panel/content/center");
            inputMsg = GetInputField("Panel/content/bottom/input");
            sendBtn = GetButton("Panel/content/bottom/send");

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
                }
                else
                {
                    SetImage(item.Icon, info.SenderFaceURL);
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
            inputMsg.onSubmit.AddListener((value) =>
            {
                TrySendTextMsg();
            });
            OnClick(sendBtn, () =>
            {
                TrySendTextMsg();
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
            GameEntry.Event.Subscribe(OnCreateGroup.EventId, HandleCreateGroup);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            msgList.Clear();
            selfUserInfo = null;
            GameEntry.Event.Unsubscribe(OnCreateGroup.EventId, HandleCreateGroup);
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

        void TrySendTextMsg()
        {
            var msg = inputMsg.text;
            if (msg == "")
            {
                return;
            }
            var msgStruct = IMSDK.CreateTextMessage(msg);
            IMSDK.SendMessage((msg, errCode, errMsg) =>
            {
                if (msg != null)
                {
                    msgList.Add(msg);
                    RefreshList(chatList, msgList.Count);
                }
                else
                {
                    Debug.LogError(errCode + "" + errMsg);
                }
            }, msgStruct, conversation.UserID, "", new OfflinePushInfo()
            {
            });
            inputMsg.text = "";
        }

        void HandleCreateGroup(object sender, GameEventArgs e)
        {
            var args = e as OnCreateGroup;
            if (args.OldConversation != null && args.NewConversation != null && args.OldConversation.ConversationID == conversation.ConversationID)
            {
                conversation = args.NewConversation;
                RefreshUI();
            }
        }
    }
}

