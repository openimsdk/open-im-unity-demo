using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public class UIChat : UGuiForm
    {
        Button backBtn;
        Button userInfoBtn;
        TextMeshProUGUI userName;
        LoopListView2 chatList;
        TMP_InputField inputMsg;
        Button sendBtn;

        LocalFriend selectFriend = null;
        List<MsgStruct> msgList;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/content/top/back");
            userName = GetTextPro("Panel/content/top/username");
            userInfoBtn = GetButton("Panel/content/top/userinfo");
            chatList = GetListView("Panel/content/center");
            inputMsg = GetInputField("Panel/content/bottom/input");
            sendBtn = GetButton("Panel/content/bottom/send");

            msgList = new List<MsgStruct>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            selectFriend = userData as LocalFriend;

            userName.text = selectFriend.FriendUserID;
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
                bool isSelf = info.SendID == Player.Instance.UserId;
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
                    itemNode.UserObjectData = new ChatItem()
                    {
                        Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                        Message = itemNode.transform.Find("msg/txt").GetComponent<TextMeshProUGUI>(),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                ChatItem item = itemNode.UserObjectData as ChatItem;
                if (info.TextElem != null)
                {
                    item.Message.text = info.TextElem.Content;
                }
                return itemNode;
            });
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
            RefreshList(chatList, msgList.Count);
            var conversation = Player.Instance.Conversation.GetFriendConversation(selectFriend.FriendUserID);
            if (conversation != null)
            {
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

            inputMsg.ActivateInputField();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            chatList.Reset();
            msgList.Clear();
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
            }, msgStruct, selectFriend.FriendUserID, "", new OfflinePushInfo()
            {
            });
            inputMsg.text = "";
            inputMsg.ActivateInputField();
        }
    }
}

