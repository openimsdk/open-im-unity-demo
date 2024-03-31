using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;
using System.Collections.Generic;
using System;

public class FriendItem
{
    public Button Btn;
    public Image Bg;
    public Image Icon;
    public TextMeshProUGUI Name;
}
public class ChatItem
{
    public Image Icon;
    public TextMeshProUGUI Message;
}

public partial class UIMain
{
    Transform friendRoot;
    LoopListView2 friendList;
    LoopListView2 chatList;
    TMP_InputField inputMsg;
    Button sendMsgBtn;

    LocalFriend selectFriend = null;
    List<MsgStruct> msgList;
    void InitFriendUI()
    {
        msgList = new List<MsgStruct>();
        friendRoot = transform.Find("content/friend");
        friendList = friendRoot.Find("list").GetComponent<LoopListView2>();
        chatList = friendRoot.Find("chat/list").GetComponent<LoopListView2>();
        inputMsg = friendRoot.Find("chat/bottom/input").GetComponent<TMP_InputField>();
        sendMsgBtn = friendRoot.Find("chat/bottom/send").GetComponent<Button>();

        friendList.InitListView(0, (list, index) =>
        {
            if (index < 0)
            {
                return null;
            }
            var itemNode = list.NewListViewItem("item");
            if (!itemNode.IsInitHandlerCalled)
            {
                itemNode.UserObjectData = new FriendItem()
                {
                    Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                    Name = itemNode.transform.Find("name").GetComponent<TextMeshProUGUI>(),
                    Bg = itemNode.transform.Find("bg").GetComponent<Image>(),
                    Btn = itemNode.transform.GetComponent<Button>(),
                };
                itemNode.IsInitHandlerCalled = true;
            }
            FriendItem item = itemNode.UserObjectData as FriendItem;
            var info = Player.CurPlayer.Friend.FriendList[index];
            // Debug.Log(JsonUtility.ToJson(info));
            item.Name.text = info.FriendUserID;
            if (selectFriend != null && selectFriend == info)
            {
                item.Bg.color = new Color(0.1f, 1f, 0.3f, 1);
            }
            else
            {
                item.Bg.color = Color.gray;
            }
            OnClick(item.Btn, () =>
            {
                selectFriend = info;
                RefreshFriendList();
                RefreshChatList();
            });
            return itemNode;
        });

        chatList.InitListView(0, (list, index) =>
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 itemNode = null;
            var info = msgList[index];
            bool isSelf = info.SendID == Player.CurPlayer.UserId;
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
        chatList.SetListItemCount(0);

        inputMsg.onSubmit.AddListener((value) =>
        {
            TrySendTextMsg();
        });

        OnClick(sendMsgBtn, () =>
        {
            TrySendTextMsg();
        });
    }

    void TrySendTextMsg()
    {
        var msg = inputMsg.text;
        if (msg == "")
        {
            return;
        }
        var msgStruct = IMSDK.CreateTextMessage(msg);
        Debug.Log(msgStruct);
        IMSDK.SendMessage((msg, errCode, errMsg) =>
        {
            if (msg != null)
            {
                msgList.Add(msg);
                RefreshChatList();
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

    void RefreshFriendList()
    {
        friendList.SetListItemCount(Player.CurPlayer.Friend.FriendList.Count);
        friendList.RefreshAllShownItem();
    }

    void RefreshChatList()
    {
        if (selectFriend != null)
        {
            var conversation = Player.CurPlayer.Conversation.GetFriendConversation(selectFriend.FriendUserID);
            if (conversation == null)
            {
                chatList.SetListItemCount(0);
            }
            else
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
                        chatList.SetListItemCount(msgList.Count);
                        chatList.MovePanelToItemIndex(msgList.Count, 0);
                    }
                    else
                    {
                        chatList.SetListItemCount(0);
                    }
                }, new GetAdvancedHistoryMessageListParams()
                {
                    UserID = conversation.UserID,
                    ConversationID = conversation.ConversationID,
                    Count = 10,
                });
            }
        }
        chatList.RefreshAllShownItem();
    }

    void OnConverationChange(LocalConversation conversation)
    {
        if (selectFriend != null && conversation.UserID == selectFriend.FriendUserID)
        {
            RefreshChatList();
        }
    }
}