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

    public class UINewFriend : UGuiForm
    {
        class UserRequestItem
        {
            public Image Icon;
            public TextMeshProUGUI Name;
            public TextMeshProUGUI Tip;
            public RectTransform MenuRect;
            public Button AcceptBtn;
            public Button RefuseBtn;
        }
        Button backBtn;
        LoopListView2 requestList;
        List<LocalFriendRequest> requestInfoList;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            requestList = GetListView("Panel/content/list");
            requestInfoList = new List<LocalFriendRequest>();
            requestList.InitListView(0, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                if (requestInfoList.Count <= index)
                {
                    return null;
                }
                LoopListViewItem2 itemNode = list.NewListViewItem("item");
                var info = requestInfoList[index];
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new UserRequestItem()
                    {
                        Icon = GetImage("icon", parent),
                        Name = GetTextPro("name", parent),
                        Tip = GetTextPro("tip", parent),
                        MenuRect = GetRectTransform("menu", parent),
                        AcceptBtn = GetButton("menu/accept", parent),
                        RefuseBtn = GetButton("menu/refuse", parent)
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                var item = itemNode.UserObjectData as UserRequestItem;
                OnClick(item.AcceptBtn, () =>
                {
                    IMSDK.AcceptFriendApplication((suc, errCode, errMsg) =>
                    {
                        if (suc)
                        {
                            RefreshList(requestList, requestInfoList.Count);
                        }
                        else
                        {
                            Debug.Log(errCode + ":" + errMsg);
                        }
                    }, new ProcessFriendApplicationParams()
                    {
                        ToUserID = info.FromUserID
                    });
                });
                OnClick(item.RefuseBtn, () =>
                {
                    IMSDK.RefuseFriendApplication((suc, errCode, errMsg) =>
                    {
                        if (suc)
                        {

                        }
                        else
                        {
                            Debug.Log(errCode + ":" + errMsg);
                        }
                    }, new ProcessFriendApplicationParams()
                    {
                        ToUserID = info.FromUserID
                    });
                });
                item.Name.text = null;
                item.Icon.sprite = null;
                if (info.FromUserID == IMSDK.GetLoginUser())
                {
                    item.Name.text = info.ToNickname;
                    SetImage(item.Icon, info.ToFaceURL);
                    item.MenuRect.gameObject.SetActive(false);
                    if (info.HandleResult == (int)HandleResult.Unprocessed)
                    {
                        item.Tip.text = "待同意";
                    }
                    else if (info.HandleResult == (int)HandleResult.Agree)
                    {
                        item.Tip.text = "已同意";
                    }
                    else if (info.HandleResult == (int)HandleResult.Reject)
                    {
                        item.Tip.text = "已拒绝";
                    }
                }
                else
                {
                    item.Name.text = info.FromNickname;
                    SetImage(item.Icon, info.FromFaceURL);
                    if (info.HandleResult == (int)HandleResult.Unprocessed)
                    {
                        item.Tip.text = "";
                        item.MenuRect.gameObject.SetActive(true);
                    }
                    else if (info.HandleResult == (int)HandleResult.Agree)
                    {
                        item.Tip.text = "已同意";
                        item.MenuRect.gameObject.SetActive(false);
                    }
                    else if (info.HandleResult == (int)HandleResult.Reject)
                    {
                        item.Tip.text = "已拒绝";
                        item.MenuRect.gameObject.SetActive(false);
                    }
                }
                return itemNode;
            });
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            Refresh();
            GameEntry.Event.Subscribe(OnFriendChange.EventId, HandleFriendChange);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(OnFriendChange.EventId, HandleFriendChange);
        }

        void Refresh()
        {
            requestInfoList.Clear();
            IMSDK.GetFriendApplicationListAsRecipient((list, errCode, errMsg) =>
            {
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        requestInfoList.AddRange(list);
                        RefreshList(requestList, requestInfoList.Count);
                    }
                }
                else
                {
                    GameEntry.UI.Tip(errMsg);
                }
            });

            IMSDK.GetFriendApplicationListAsApplicant((list, errCode, errMsg) =>
            {
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        requestInfoList.AddRange(list);
                        RefreshList(requestList, requestInfoList.Count);
                    }
                }
                else
                {
                    GameEntry.UI.Tip(errMsg);
                }
            });
        }

        private void HandleFriendChange(object sender, GameEventArgs e)
        {
            var args = e as OnFriendChange;
            if (args.Operation == FriendOperation.ApplicationAccepted || args.Operation == FriendOperation.ApplicationAdded || args.Operation == FriendOperation.ApplicationDeleted || args.Operation == FriendOperation.ApplicationRejected)
            {
                Refresh();
            }
        }
    }
}

