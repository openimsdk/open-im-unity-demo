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
        public enum RequestHistoryStatus
        {
            None, Done, Requesting,
        }
        public class ChatItem
        {
            public RectTransform Rect;
            public Button Btn;
            public Image Icon;
            public RectTransform ContentRect;
            public Image ContentArrow;
            public Image ContentBg;
            public RectTransform ContentBgRect;
            public TextMeshProUGUI ContentStr;
            public RectTransform ContentStrRect;
            public TextMeshProUGUI ReadStatus;
            public ContentSizeFitter ContentSizeFitter;
            public LayoutElement LayoutElement;
        }
        Button backBtn;
        Button chatInfoBtn;
        TextMeshProUGUI userName;
        LoopListView2 chatList;
        LoopListView2 chatList_topdown;
        TMP_InputField msgInput;
        List<MsgStruct> msgList;
        RectTransform centerRect;
        RectTransform listRect;
        RectTransform contentRect;
        LocalConversation conversation;
        LocalUser selfUserInfo;

        RequestHistoryStatus requestHistoryStatus = RequestHistoryStatus.None;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/content/top/back");
            userName = GetTextPro("Panel/content/top/username");
            chatInfoBtn = GetButton("Panel/content/top/chatinfo");
            chatList = GetListView("Panel/content/center/list");
            chatList_topdown = GetListView("Panel/content/center/listtopdown");
            msgInput = GetInputField("Panel/content/center/input/input");
            centerRect = GetRectTransform("Panel/content/center");
            listRect = GetRectTransform("Panel/content/center/list");
            contentRect = GetRectTransform("Panel/content/center/list/Viewport/Content");
            msgList = new List<MsgStruct>();
            chatList.InitListView(0, (list, index) =>
            {
                if (index < 0) return null;
                if (msgList.Count <= index) return null;
                LoopListViewItem2 itemNode = null;
                var msgStruct = msgList[index];
                var isSelf = false;
                isSelf = msgStruct.SendID == IMSDK.GetLoginUser();
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
                    itemNode.UserObjectData = RegisterChatItem(parent);
                    itemNode.IsInitHandlerCalled = true;
                }
                ChatItem item = itemNode.UserObjectData as ChatItem;
                SetChatItemInfo(item, msgStruct, isSelf);
                return itemNode;
            });
            chatList.mOnBeginDragAction = OnBeginDrag;
            chatList.mOnDragingAction = OnDraging;
            chatList.mOnEndDragAction = OnEndDrag;
            chatList_topdown.InitListView(0, (list, index) =>
            {
                if (index < 0) return null;
                if (msgList.Count <= index) return null;
                LoopListViewItem2 itemNode = null;
                var msgStruct = msgList[(msgList.Count - 1) - index];
                var isSelf = false;
                isSelf = msgStruct.SendID == IMSDK.GetLoginUser();
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
                    itemNode.UserObjectData = RegisterChatItem(parent);
                    itemNode.IsInitHandlerCalled = true;
                }
                ChatItem item = itemNode.UserObjectData as ChatItem;
                SetChatItemInfo(item, msgStruct, isSelf);
                return itemNode;
            });
        }
        ChatItem RegisterChatItem(RectTransform parent)
        {
            var node = new ChatItem()
            {
                Rect = parent,
                Btn = GetButton("head", parent),
                Icon = GetImage("head/icon", parent),
                ContentRect = GetRectTransform("content", parent),
                ContentArrow = GetImage("content/arrow", parent),
                ContentBg = GetImage("content/bg", parent),
                ContentBgRect = GetRectTransform("content/bg", parent),
                ContentStr = GetTextPro("content/strMsg", parent),
                ContentStrRect = GetRectTransform("content/strMsg", parent),
                ReadStatus = GetTextPro("head/readstatus", parent),
            };
            node.ContentSizeFitter = node.ContentStrRect.GetComponent<ContentSizeFitter>();
            node.LayoutElement = node.ContentStrRect.GetComponent<LayoutElement>();
            return node;
        }
        void SetChatItemInfo(ChatItem item, MsgStruct msgStruct, bool isSelf)
        {
            if (msgStruct.TextElem != null)
            {
                item.ContentStr.text = msgStruct.TextElem.Content;
            }
            else
            {
                item.ContentStr.text = "";
            }
            item.LayoutElement.preferredWidth = Mathf.Clamp(item.ContentStr.preferredWidth, 100, 500);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.ContentStrRect);
            var contentSize = item.ContentStrRect.sizeDelta;
            item.ContentBgRect.sizeDelta = contentSize + new Vector2(30, 30);
            item.ContentRect.sizeDelta = item.ContentBgRect.sizeDelta;
            var y = item.ContentRect.sizeDelta.y + 30;
            if (y < 130)
            {
                y = 130;
            }
            item.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            if (isSelf)
            {
                item.ContentArrow.color = Color.white;
                item.ContentBg.color = Color.white;
                if (conversation.ConversationType == (int)ConversationType.Single)
                {
                    item.ReadStatus.text = msgStruct.IsRead ? "已读" : "未读";
                }
                else
                {
                    item.ReadStatus.text = "";
                }
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
                item.ContentArrow.color = new Color32(160, 231, 90, 255);
                item.ContentBg.color = new Color32(160, 231, 90, 255);
                SetImage(item.Icon, msgStruct.SenderFaceURL);
                OnClick(item.Btn, () =>
                {
                    GameEntry.UI.OpenUI("UserInfo", msgStruct.SendID);
                });
            }
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
            GameEntry.Event.Subscribe(OnAdvancedMsg.EventId, HandleOnAdvancedMsg);

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
            GameEntry.Event.Unsubscribe(OnAdvancedMsg.EventId, HandleOnAdvancedMsg);
        }
        void OnBeginDrag()
        {

        }

        void OnDraging()
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(UIExtension.UICamera, listRect.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(contentRect, screenPos, UIExtension.UICamera, out var lcoalPos))
            {
                if (Mathf.Abs(contentRect.sizeDelta.y - lcoalPos.y) < 200)
                {
                    Debug.Log("Request List");
                    if (msgList.Count > 0)
                    {
                        var msgStruct = msgList[msgList.Count - 1];
                        ReqeustHistory(msgStruct.ClientMsgID, msgStruct.Seq);
                    }
                }
            }

        }
        void OnEndDrag()
        {

        }
        void RefreshUI()
        {
            chatList.gameObject.SetActive(false);
            chatList_topdown.gameObject.SetActive(false);
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
                msgList.Clear();
                ReqeustHistory("", 0);
            }
        }

        void ReqeustHistory(string startMsgId, long lastMinSeq, int count = 20)
        {
            if (requestHistoryStatus == RequestHistoryStatus.Requesting) return;
            requestHistoryStatus = RequestHistoryStatus.Requesting;

            IMSDK.GetAdvancedHistoryMessageList((list, err, msg) =>
            {
                if (list != null)
                {
                    for (int i = (list.MessageList.Length - 1); i >= 0; i--)
                    {
                        msgList.Add(list.MessageList[i]);
                    }
                }
                chatList.gameObject.SetActive(true);
                RefreshList(chatList, msgList.Count);
                if (startMsgId == "")
                {
                    chatList.MovePanelToItemIndex(0, 0);
                }
                if (contentRect.sizeDelta.y < listRect.rect.height)
                {
                    chatList.gameObject.SetActive(false);
                    chatList_topdown.gameObject.SetActive(true);
                    RefreshList(chatList_topdown, msgList.Count, true);
                }
                else
                {
                    chatList_topdown.gameObject.SetActive(false);
                    chatList.gameObject.SetActive(true);
                }
                requestHistoryStatus = RequestHistoryStatus.Done;
            }, new GetAdvancedHistoryMessageListParams()
            {
                UserID = conversation.UserID,
                ConversationID = conversation.ConversationID,
                Count = count,
                StartClientMsgID = startMsgId,
                LastMinSeq = lastMinSeq,
            });
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
                    msgList.Insert(0, msg);
                    chatList.gameObject.SetActive(true);
                    RefreshList(chatList, msgList.Count);
                    chatList.MovePanelToItemIndex(0, 0);
                    if (contentRect.sizeDelta.y < listRect.rect.height)
                    {
                        chatList.gameObject.SetActive(false);
                        chatList_topdown.gameObject.SetActive(true);
                        RefreshList(chatList_topdown, msgList.Count, true);
                    }
                    else
                    {
                        chatList_topdown.gameObject.SetActive(false);
                        chatList.gameObject.SetActive(true);
                    }
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

        void HandleOnAdvancedMsg(object sender, GameEventArgs e)
        {
            var args = e as OnAdvancedMsg;
            if (args.AdvancedMsgOperation == AdvancedMsgOperation.C2CReadReceipt || args.AdvancedMsgOperation == AdvancedMsgOperation.GroupReadReceipt)
            {
                if (args.MsgReceipts != null && args.MsgReceipts.Count > 0)
                {
                    foreach (var msgStruct in msgList)
                    {
                        foreach (var receipt in args.MsgReceipts)
                        {
                            if (msgStruct.RecvID == receipt.UserID)
                            {
                                msgStruct.IsRead = true;
                            }
                        }
                    }
                    if (chatList.gameObject.activeSelf)
                    {
                        chatList.RefreshAllShownItem();
                    }
                    else
                    {
                        chatList_topdown.RefreshAllShownItem();
                    }
                }
            }
        }
    }
}

